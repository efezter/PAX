import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TaskStatusesServiceProxy, TaskStatusDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditTaskStatusModalComponent } from './create-or-edit-taskStatus-modal.component';

import { ViewTaskStatusModalComponent } from './view-taskStatus-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

import { ChangeIconModalComponent } from './change-icon-modal.component';

@Component({
    templateUrl: './taskStatuses.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class TaskStatusesComponent extends AppComponentBase {
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('createOrEditTaskStatusModal', { static: true })
    createOrEditTaskStatusModal: CreateOrEditTaskStatusModalComponent;
    @ViewChild('viewTaskStatusModalComponent', { static: true }) viewTaskStatusModal: ViewTaskStatusModalComponent;
    @ViewChild('changeIconModalComponent', { static: true }) changeIconModal: ChangeIconModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    serverUrl = AppConsts.remoteServiceBaseUrl;

    _entityTypeFullName = 'PAX.Next.TaskManager.TaskStatus';
    entityHistoryEnabled = false;

    constructor(
        injector: Injector,
        private _taskStatusesServiceProxy: TaskStatusesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.entityHistoryEnabled = this.setIsEntityHistoryEnabled();
    }

    private setIsEntityHistoryEnabled(): boolean {
        let customSettings = (abp as any).custom;
        return (
            this.isGrantedAny('Pages.Administration.AuditLogs') &&
            customSettings.EntityHistory &&
            customSettings.EntityHistory.isEnabled &&
            _filter(
                customSettings.EntityHistory.enabledEntities,
                (entityType) => entityType === this._entityTypeFullName
            ).length === 1
        );
    }

    getTaskStatuses(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();
       
        this._taskStatusesServiceProxy
            .getAll(
                this.filterText,
                this.nameFilter,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getMaxResultCount(this.paginator, event)
            )
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createTaskStatus(): void {
        this.createOrEditTaskStatusModal.show();
    }

    showHistory(taskStatus: TaskStatusDto): void {
        this.entityTypeHistoryModal.show({
            entityId: taskStatus.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: '',
        });
    }

    deleteTaskStatus(taskStatus: TaskStatusDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._taskStatusesServiceProxy.delete(taskStatus.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._taskStatusesServiceProxy.getTaskStatusesToExcel(this.filterText, this.nameFilter).subscribe((result) => {
            this._fileDownloadService.downloadTempFile(result);
        });
    }
}
