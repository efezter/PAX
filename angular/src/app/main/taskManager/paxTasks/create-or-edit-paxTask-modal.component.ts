import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    PaxTasksServiceProxy,
    CreateOrEditPaxTaskDto,
    PaxTaskSeverityLookupTableDto,
    PaxTaskTaskStatusLookupTableDto,
    TaskType,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { PaxTaskUserLookupTableModalComponent } from './paxTask-user-lookup-table-modal.component';

import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';

@Component({
    selector: 'createOrEditPaxTaskModal',
    templateUrl: './create-or-edit-paxTask-modal.component.html',
})
export class CreateOrEditPaxTaskModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('paxTaskUserLookupTableModal', { static: true })
    paxTaskUserLookupTableModal: PaxTaskUserLookupTableModalComponent;
    @ViewChild('paxTaskUserLookupTableModal2', { static: true })
    paxTaskUserLookupTableModal2: PaxTaskUserLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    public Editor = ClassicEditor;

    active = false;
    saving = false;

    paxTask: CreateOrEditPaxTaskDto = new CreateOrEditPaxTaskDto();

    reporterName = '';
    assigneeName = '';
    severityName = '';
    taskStatusName = '';

    allSeveritys: PaxTaskSeverityLookupTableDto[];
    allTaskStatuss: PaxTaskTaskStatusLookupTableDto[];

    constructor(
        injector: Injector,
        private _paxTasksServiceProxy: PaxTasksServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(paxTaskId?: number): void {
        if (!paxTaskId) {
            this.paxTask = new CreateOrEditPaxTaskDto();
            this.paxTask.id = paxTaskId;
            this.paxTask.taskType = TaskType.Normal;
            this.reporterName = '';
            this.assigneeName = '';
            this.severityName = '';
            this.taskStatusName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._paxTasksServiceProxy.getPaxTaskForEdit(paxTaskId).subscribe((result) => {
                this.paxTask = result.paxTask;

                this.reporterName = result.userName;
                this.assigneeName = result.userName2;
                this.severityName = result.severityName;
                this.taskStatusName = result.taskStatusName;

                this.active = true;
                this.modal.show();
            });
        }
        this._paxTasksServiceProxy.getAllSeverityForTableDropdown().subscribe((result) => {
            this.allSeveritys = result;
        });
        this._paxTasksServiceProxy.getAllTaskStatusForTableDropdown().subscribe((result) => {
            this.allTaskStatuss = result;
        });
    }

    save(): void {
        this.saving = true;

        this._paxTasksServiceProxy
            .createOrEdit(this.paxTask)
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

    openSelectUserModal() {
        this.paxTaskUserLookupTableModal.id = this.paxTask.reporterId;
        this.paxTaskUserLookupTableModal.displayName = this.reporterName;
        this.paxTaskUserLookupTableModal.show();
    }
    openSelectUserModal2() {
        this.paxTaskUserLookupTableModal2.id = this.paxTask.assigneeId;
        this.paxTaskUserLookupTableModal2.displayName = this.reporterName;
        this.paxTaskUserLookupTableModal2.show();
    }

    setReporterIdNull() {
        this.paxTask.reporterId = null;
        this.reporterName = '';
    }
    setAssigneeIdNull() {
        this.paxTask.assigneeId = null;
        this.assigneeName = '';
    }

    getNewReporterId() {
        this.paxTask.reporterId = this.paxTaskUserLookupTableModal.id;
        this.reporterName = this.paxTaskUserLookupTableModal.displayName;
    }
    getNewAssigneeId() {
        this.paxTask.assigneeId = this.paxTaskUserLookupTableModal2.id;
        this.assigneeName = this.paxTaskUserLookupTableModal2.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
