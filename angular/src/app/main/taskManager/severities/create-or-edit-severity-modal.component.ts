import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { SeveritiesServiceProxy, CreateOrEditSeverityDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditSeverityModal',
    templateUrl: './create-or-edit-severity-modal.component.html',
})
export class CreateOrEditSeverityModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    severity: CreateOrEditSeverityDto = new CreateOrEditSeverityDto();

    constructor(
        injector: Injector,
        private _severitiesServiceProxy: SeveritiesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(severityId?: number): void {
        if (!severityId) {
            this.severity = new CreateOrEditSeverityDto();
            this.severity.id = severityId;

            this.active = true;
            this.modal.show();
        } else {
            this._severitiesServiceProxy.getSeverityForEdit(severityId).subscribe((result) => {
                this.severity = result.severity;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._severitiesServiceProxy
            .createOrEdit(this.severity)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
