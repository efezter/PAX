import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { finalize } from 'rxjs/operators';
import {
    PaxTasksServiceProxy,
    CommentsServiceProxy,
    CreateOrEditPaxTaskDto,
    PaxTaskSeverityLookupTableDto,
    PaxTaskTaskStatusLookupTableDto,
    TaskType,
    CreateOrEditCommentDto,
    GetCommentForViewDto,
    WatcherUserLookupTableDto,
    CommentDto
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { AbpSessionService } from 'abp-ng2-module';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { PaxTaskUserLookupTableModalComponent } from './paxTask-user-lookup-table-modal.component';
import { ActivatedRoute, Router } from '@angular/router';
import { AppConsts } from '@shared/AppConsts';
// import { LazyLoadEvent } from 'primeng/api';
// import { ChangeEvent } from '@ckeditor/ckeditor5-angular/ckeditor.component';
// import Mention from '@ckeditor/ckeditor5-mention/src/mention';
// import { add } from 'lodash';

import * as CustomCK from 'shared/customCK/build/ckeditor';


@Component({
    selector: 'createOrEditPaxTaskModal',
    templateUrl: './create-or-edit-paxTask.component.html',
    styleUrls: ['./create-or-edit-paxTask.component.less'],
})
export class CreateOrEditPaxTaskModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('paxTaskUserLookupTableModal', { static: true })
    paxTaskUserLookupTableModal: PaxTaskUserLookupTableModalComponent;

    public Editor = CustomCK;
    public Editorr = CustomCK;
    public Editorrr = CustomCK;
    public commentEditors: any[] = new Array<{ commentId: number, editor: any }>();

    filteredCountries: WatcherUserLookupTableDto[];
    
    countries: WatcherUserLookupTableDto[] = new Array<WatcherUserLookupTableDto>();
    commentToCreateOrEdit: CreateOrEditCommentDto = new CreateOrEditCommentDto();
    isCommentEditing: boolean = false;

    

       // Create a configuration object
editorConfiguration = {
    mention: {
        feeds: [
            {
                marker: '@',
                feed: this.getFeedItems,
                itemRenderer: this.customItemRenderer,
                minimumCharacters: 1
            }
        ]
    }
};

    active = false;
    saving = false;

    paxTask: CreateOrEditPaxTaskDto = new CreateOrEditPaxTaskDto();
    commentViews: GetCommentForViewDto[];
    showCommentCreate: boolean = false;

    cuRuserId = -1;
    reporterName = '';
    assigneeName = '';
    severityName = '';
    taskStatusName = '';

    uploadUrl: string;
    uploadedFiles: any[] = [];

    allSeveritys: PaxTaskSeverityLookupTableDto[];
    allTaskStatuss: PaxTaskTaskStatusLookupTableDto[];

    constructor(
        injector: Injector,
        private _paxTasksServiceProxy: PaxTasksServiceProxy,
        private _commentsServiceProxy: CommentsServiceProxy,
        private _dateTimeService: DateTimeService,
        private _activatedRoute: ActivatedRoute,
        private _router: Router,
        private _abpSessionService: AbpSessionService,
    ) {
        super(injector);
    }

    customItemRenderer( item ) {
        const itemElement = document.createElement( 'div' );
    
        itemElement.classList.add( 'mention-item' );
        itemElement.id = `mention-list-item-id-${ item.userId }`;
        itemElement.textContent = `${ item.id } `;

        return itemElement;
    }

    getFeedItems( queryText ) {        
       return this._paxTasksServiceProxy
        .getUsersForMention(
            queryText
        );       
    }

    // public onChange( { editor }: ChangeEvent ) {

    //     // const data = editor.getData();
    //     editor.ui.view.toolbar.element.style.display = 'none';
    //     // console.log( data );
    // }
    

    ngOnInit(): void {
        // ClassicEditor.builtinPlugins = [ Mention ];
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/PaxTask/UploadFiles';

        this.cuRuserId = this._abpSessionService.userId;
        let paxTaskId = parseInt(this._activatedRoute.snapshot.queryParams['taskId']);
        if (!paxTaskId) {
            this.paxTask = new CreateOrEditPaxTaskDto();
            this.paxTask.id = paxTaskId;
            this.paxTask.taskType = TaskType.Normal;
            this.reporterName = '';
            this.assigneeName = '';
            this.severityName = '';
            this.taskStatusName = '';

            this.active = true;
        } else {
            this.getTaskDetails(paxTaskId);
        }
        this._paxTasksServiceProxy.getAllSeverityForTableDropdown().subscribe((result) => {
            this.allSeveritys = result;
        });
        this._paxTasksServiceProxy.getAllTaskStatusForTableDropdown().subscribe((result) => {
            this.allTaskStatuss = result;
        });
    }

    

    public onReady(ckEditor, commentId) {

        ckEditor._paxTasksServiceProxy = this._paxTasksServiceProxy;
        
        if (commentId != 0) {//Edit editors
            ckEditor.ui.view.toolbar.element.style.display = 'none';
            ckEditor.isReadOnly = true;
            this.commentEditors.push({ commentId: commentId, editor: ckEditor });
        }        
    }

    public prepareForEdit(commentId: number, toggle: boolean) {

        let editor = this.commentEditors.find(e => e.commentId == commentId).editor;

        if (!toggle) {
            editor.ui.view.toolbar.element.style.display = 'none';
            editor.isReadOnly = true;
            this.isCommentEditing = false;
        } else {
            editor.ui.view.toolbar.element.style.display = 'flex';
            editor.isReadOnly = false;
            this.isCommentEditing = true;
            if (this.showCommentCreate == true) {
                this.showCommentCreate = false;
            }
        }
    }

    getUsers(event) {

        this._paxTasksServiceProxy
            .getAllUserForLookupTable(
                event.query,
                undefined,
                undefined,
                100,
                this.countries.map(a => a.id)
            )
            .subscribe((result) => {
                this.filteredCountries = result.items;
            });
    }

    save(): void {

        this.saving = true;

        this.paxTask.watchers = this.countries;

        this._paxTasksServiceProxy
            .createOrEdit(this.paxTask)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                // this.close();
            });
    }

    openSelectUserModal() {
        this.paxTaskUserLookupTableModal.id = this.paxTask.assigneeId;
        this.paxTaskUserLookupTableModal.displayName = this.assigneeName;
        this.paxTaskUserLookupTableModal.show();
    }

    setAssigneeIdNull() {
        this.paxTask.assigneeId = null;
        this.assigneeName = '';
    }

    getNewAssigneeId() {
        this.paxTask.assigneeId = this.paxTaskUserLookupTableModal.id;
        this.assigneeName = this.paxTaskUserLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this._router.navigate(['app/main/taskManager/paxTasks']);
    }

    getTaskDetails(paxTaskId: number): void {

        this._paxTasksServiceProxy.getPaxTaskForEdit(paxTaskId).subscribe((result) => {
            this.paxTask = result.paxTask;

            this.countries = result.paxTask.watchers;

            this.reporterName = result.userName;
            this.assigneeName = result.userName2;
            this.severityName = result.severityName;
            this.taskStatusName = result.taskStatusName;

            this.getTaskComments();
        });

    }

    getTaskComments(): void {

        this._commentsServiceProxy.getAll(
            undefined,
            this.paxTask.id,
            undefined,
            undefined,
            undefined,
            0,
            10
        ).subscribe((result) => {
            this.commentViews = result.items;
            this.active = true;
        });
    }

    toggleCommentCreate(): void {
        this.showCommentCreate = !this.showCommentCreate;
    }

    saveComment(comment: CommentDto): void {

        this.saving = true;

        if (!comment) {
            this.commentToCreateOrEdit.paxTaskId = this.paxTask.id;
        }
        else {
            this.commentToCreateOrEdit.paxTaskId = this.paxTask.id;
            this.commentToCreateOrEdit.id = comment.id;
            this.commentToCreateOrEdit.commentText = comment.commentText;
        }

        this._commentsServiceProxy
            .createOrEdit(this.commentToCreateOrEdit)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((res) => {

                if (!comment) {

                    let added = new GetCommentForViewDto();

                    added.creationTime = DateTime.now();
                    added.comment = new CommentDto();
                    added.comment.id = res.id;
                    added.comment.commentText = res.commentText;
                    added.comment.userId = this._abpSessionService.userId;
                    this.commentViews.unshift(added)

                    this.showCommentCreate = false;
                    this.commentToCreateOrEdit = new CreateOrEditCommentDto();
                    this.commentToCreateOrEdit.commentText = "";

                } else {
                    this.prepareForEdit(comment.id, false);
                }

                this.notify.info(this.l('SavedSuccessfully'));
            });
    }

    deleteComment(commentId: number): void {

        this.message.confirm(
            this.l('DeleteCommentMessage'),
            this.l('AreYouSure'),
            isConfirmed => {
                if (isConfirmed) {
                    this.saving = true;

                    this._commentsServiceProxy
                        .delete(commentId)
                        .pipe(
                            finalize(() => {
                                this.saving = false;
                            })
                        )
                        .subscribe((res) => {

                            let deletedItem = this.commentViews.find(e => e.comment.id = commentId);

                            this.commentViews.splice(this.commentViews.indexOf(deletedItem), 1)

                            this.notify.info(this.l('SavedSuccessfully'));
                        });
                }
            }
        );
    }

      // upload completed event
      onUpload(event): void {
        for (const file of event.files) {
            this.uploadedFiles.push(file);
        }
    }

    onBeforeSend(event): void {
        event.xhr.setRequestHeader('Authorization', 'Bearer ' + abp.auth.getToken());
    }
}
