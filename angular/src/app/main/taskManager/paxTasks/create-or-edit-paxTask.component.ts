import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { finalize } from 'rxjs/operators';
import {
    PaxTasksServiceProxy,
    CreateOrEditPaxTaskDto,
    PaxTaskSeverityLookupTableDto,
    PaxTaskTaskStatusLookupTableDto,
    TaskType,
    UserServiceProxy,
    PaxTaskUserLookupTableDto,
    WatcherUserLookupTableDto
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { PaxTaskUserLookupTableModalComponent } from './paxTask-user-lookup-table-modal.component';
import { ActivatedRoute, Router } from '@angular/router';
import { DemoUiComponentsServiceProxy, NameValueOfString } from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent } from 'primeng/api';

import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';

@Component({
    selector: 'createOrEditPaxTaskModal',
    templateUrl: './create-or-edit-paxTask.component.html',
})
export class CreateOrEditPaxTaskModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('paxTaskUserLookupTableModal', { static: true })
    paxTaskUserLookupTableModal: PaxTaskUserLookupTableModalComponent;
    @ViewChild('paxTaskUserLookupTableModal2', { static: true })
    paxTaskUserLookupTableModal2: PaxTaskUserLookupTableModalComponent;

    public Editor = ClassicEditor;

    filteredCountries: WatcherUserLookupTableDto[];
    countries: WatcherUserLookupTableDto[] = new Array<WatcherUserLookupTableDto>();

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
        private demoUiComponentsService: DemoUiComponentsServiceProxy,
        private _paxTasksServiceProxy: PaxTasksServiceProxy,
        private _dateTimeService: DateTimeService,
        private _activatedRoute: ActivatedRoute,
        private _router: Router,
        private _userServiceProxy: UserServiceProxy,
    ) {
        super(injector);
    }

    // filterCountries(event): void {
    //     this.demoUiComponentsService.getCountries(event.query).subscribe(countries => {
    //         this.filteredCountries = countries;
    //     });
    // }

    getUsers(event) {

        this._paxTasksServiceProxy
            .getAllUserForLookupTable(
                event.query,
                undefined,
                undefined,
                100
            )
            .subscribe((result) => {
              this.filteredCountries = result.items;
            });
        // this._userServiceProxy.getUsers(new GetUsersInput({
        //     filter: event.query,
        //     permissions: undefined,
        //     role: undefined,
        //     onlyLockedUsers: false,
        //     sorting: undefined,
        //     maxResultCount: 100,
        //     skipCount: 0
        // })
        // ).pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator())).subscribe(result => {
        //     debugger;
        //     this.filteredCountries = result.items;
        // });
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
        this._router.navigate(['app/main/taskManager/paxTasks']);
    }

    ngOnInit(): void {

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
            this._paxTasksServiceProxy.getPaxTaskForEdit(paxTaskId).subscribe((result) => {
                this.paxTask = result.paxTask;

                this.countries = result.paxTask.watchers;

                this.reporterName = result.userName;
                this.assigneeName = result.userName2;
                this.severityName = result.severityName;
                this.taskStatusName = result.taskStatusName;

                this.active = true;
            });
        }
        this._paxTasksServiceProxy.getAllSeverityForTableDropdown().subscribe((result) => {
            this.allSeveritys = result;
        });
        this._paxTasksServiceProxy.getAllTaskStatusForTableDropdown().subscribe((result) => {
            this.allTaskStatuss = result;
        });
    }
}
