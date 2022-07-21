import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter,ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetSeverityForViewDto, SeveritiesServiceProxy, SeverityDto, UpdateIconInput } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

import {FileUploader, FileUploaderOptions, FileItem} from 'ng2-file-upload';
import {IAjaxResponse, TokenService, AbpSessionService} from 'abp-ng2-module';
import {finalize} from 'rxjs/operators';
import {ImageCroppedEvent, base64ToFile} from 'ngx-image-cropper';

@Component({
    selector: 'changeIconModal',
    templateUrl: './change-icon-modal.component.html',
})
export class ChangeIconModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('uploadProfilePictureInputLabel') uploadProfilePictureInputLabel: ElementRef;
    

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    public useGravatarProfilePicture = false;
    public uploader: FileUploader;
    public maxProfilPictureBytesUserFriendlyValue = 5;
    public temporaryPictureUrl: string;

    private _uploaderOptions: FileUploaderOptions = {};

    active = false;
    saving = false;
    imageChangedEvent: any = '';
  
    item: GetSeverityForViewDto;

    constructor(injector: Injector,
            private _severitiesServiceProxy: SeveritiesServiceProxy,
            private _tokenService: TokenService,
            private _sessionService: AbpSessionService
            ) {
        super(injector);
        this.item = new GetSeverityForViewDto();
        this.item.severity = new SeverityDto();
    }

    initializeModal(): void {
        this.active = true;
        this.temporaryPictureUrl = '';
        this.useGravatarProfilePicture = this.setting.getBoolean('App.UserManagement.UseGravatarProfilePicture');
        if (!this.canUseGravatar()) {
            this.useGravatarProfilePicture = false;
        }
        this.initFileUploader();
    }

    initFileUploader(): void {
        this.uploader = new FileUploader({url: AppConsts.remoteServiceBaseUrl + '/Profile/UploadProfilePicture'});
        this._uploaderOptions.autoUpload = false;
        this._uploaderOptions.authToken = 'Bearer ' + this._tokenService.getToken();
        this._uploaderOptions.removeAfterUpload = true;
        this.uploader.onAfterAddingFile = (file) => {
            file.withCredentials = false;
        };

        this.uploader.onBuildItemForm = (fileItem: FileItem, form: any) => {
            form.append('FileType', fileItem.file.type);
            form.append('FileName', 'ProfilePicture');
            form.append('FileToken', this.guid());
        };

        this.uploader.onSuccessItem = (item, response, status) => {
            const resp = <IAjaxResponse>JSON.parse(response);
            if (resp.success) {
                this.updateProfilePicture(resp.result.fileToken);
            } else {
                this.message.error(resp.error.message);
            }
        };

        this.uploader.setOptions(this._uploaderOptions);
    }

    updateProfilePicture(fileToken: string): void {
        const input = new UpdateIconInput();
        input.fileToken = fileToken;
        input.x = 0;
        input.y = 0;
        input.width = 0;
        input.height = 0;
        input.taskStatusId = this.item.severity.id;

        this.saving = true;
        this._severitiesServiceProxy.updateIcon(input)
            .pipe(finalize(() => {
                this.saving = false;
            }))
            .subscribe(() => {
                this.item.severity.iconUrl = "/Common/Images/" + this._sessionService.tenantId + "/Task/SeverityIcons/" + this.item.severity.id + ".png" + '?random+\=' + Math.random();
                this.close();
            });
    }

    guid(): string {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }

        return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
    }

    save(): void {
            this.uploader.uploadAll();
    }

    canUseGravatar(): boolean {
        return this.setting.getBoolean('App.UserManagement.AllowUsingGravatarProfilePicture');
    }

    fileChangeEvent(event: any): void {
        if (event.target.files[0].size > 5242880) { //5MB
            this.message.warn(this.l('ProfilePicture_Warn_SizeLimit', this.maxProfilPictureBytesUserFriendlyValue));
            return;
        }

        this.uploadProfilePictureInputLabel.nativeElement.innerText = event.target.files[0].name;

        this.imageChangedEvent = event;
    }

    imageCroppedFile(event: ImageCroppedEvent) {
        this.uploader.clearQueue();
        this.uploader.addToQueue([<File>base64ToFile(event.base64)]);
    }

    show(item: GetSeverityForViewDto): void {
        this.item = item;
        this.active = true;
        this.initializeModal();
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.imageChangedEvent = '';
        this.uploader.clearQueue();
        this.modal.hide();
    }
}