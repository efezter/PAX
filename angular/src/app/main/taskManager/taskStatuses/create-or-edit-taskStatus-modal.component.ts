﻿import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { TaskStatusesServiceProxy, CreateOrEditTaskStatusDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditTaskStatusModal',
    templateUrl: './create-or-edit-taskStatus-modal.component.html',
})
export class CreateOrEditTaskStatusModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    taskStatus: CreateOrEditTaskStatusDto = new CreateOrEditTaskStatusDto();

    constructor(
        injector: Injector,
        private _taskStatusesServiceProxy: TaskStatusesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(taskStatusId?: number): void {
        if (!taskStatusId) {
            this.taskStatus = new CreateOrEditTaskStatusDto();
            this.taskStatus.id = taskStatusId;

            this.active = true;
            this.modal.show();
        } else {
            this._taskStatusesServiceProxy.getTaskStatusForEdit(taskStatusId).subscribe((result) => {
                this.taskStatus = result.taskStatus;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._taskStatusesServiceProxy
            .createOrEdit(this.taskStatus)
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
