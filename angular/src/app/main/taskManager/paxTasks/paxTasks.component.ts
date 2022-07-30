import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { PaxTasksServiceProxy, PaxTaskDto, TaskType, TaskTypePeriod } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { LocalStorageService } from '@shared/utils/local-storage.service';


@Component({
    templateUrl: './paxTasks.component.html',
    styleUrls: ['./paxTasks.component.less'],
    encapsulation: ViewEncapsulation.None,    
    animations: [appModuleAnimation()],
})
export class PaxTasksComponent extends AppComponentBase {
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    serverUrl = AppConsts.remoteServiceBaseUrl;
    advancedFiltersAreShown = false;
    filterText = '';
    headerFilter = '';
    labelFilter = '';
    maxCreatedDateFilter: DateTime;
    minCreatedDateFilter: DateTime;
    taskTypeFilter = -1;
    taskTypePeriodFilter = -1;
    maxPeriodIntervalFilter: number;
    maxPeriodIntervalFilterEmpty: number;
    minPeriodIntervalFilter: number;
    minPeriodIntervalFilterEmpty: number;
    userNameFilter = '';
    userName2Filter = '';
    severityNameFilter = '';
    taskStatusNameFilter = '';

    taskType = TaskType;
    taskTypePeriod = TaskTypePeriod;

    _entityTypeFullName = 'PAX.Next.TaskManager.PaxTask';
    entityHistoryEnabled = false;

    displayFilters: any[];
    selectedDisplayFilter: number = 1;
    userRolesIds: number[];  

    constructor(
        injector: Injector,
        private _paxTasksServiceProxy: PaxTasksServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _localStorageService: LocalStorageService,
        private _router: Router
    ) {
        super(injector);

        this.displayFilters = [
           // {label: 'Hepsi', value: 0},
            {label: 'Bana Atananlar', value: 1},
            {label: 'Benim oluşturduklarım', value: 2} ,
            {label: 'İzleyici olduklarım', value: 4}            
        ];   
    }

    ngOnInit(): void {
        this.entityHistoryEnabled = this.setIsEntityHistoryEnabled();
        this._localStorageService.getItem(AppConsts.User.userRolesKey, (err, userRolesIds) => {
            this.userRolesIds = JSON.parse(userRolesIds);
            this.ConfigurePage();
        });
    }

    private ConfigurePage()
    {
        if( this.userRolesIds.indexOf(AppConsts.UserRoles.admin) != -1 ||
        this.userRolesIds.indexOf(AppConsts.UserRoles.dep_chief) != -1 ||
    this.userRolesIds.indexOf(AppConsts.UserRoles.dep_man) != -1)
    {
        this.displayFilters.push({label: 'Departmanım', value: 3});
    }

        if(this.userRolesIds.indexOf(AppConsts.UserRoles.admin) != -1 ||
        this.userRolesIds.indexOf(AppConsts.UserRoles.gen_man) != -1 ||
        this.userRolesIds.indexOf(AppConsts.UserRoles.gen_man_asist) != -1 ||
        this.userRolesIds.indexOf(AppConsts.UserRoles.gen_man_sec) != -1)
        {
            this.displayFilters.push({label: 'Hepsi', value: 0});
        }
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

    getRandomColor() {
        var color = Math.floor(0x1000000 * Math.random()).toString(16);
        return '#' + ('000000' + color).slice(-6);
      }

    getPaxTasks(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }
        
        this.primengTableHelper.showLoadingIndicator();

        this._paxTasksServiceProxy
            .getAll(
                this.filterText,
                this.headerFilter,
                this.labelFilter,
                this.maxCreatedDateFilter === undefined
                    ? this.maxCreatedDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxCreatedDateFilter),
                this.minCreatedDateFilter === undefined
                    ? this.minCreatedDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minCreatedDateFilter),
                this.taskTypeFilter,
                this.taskTypePeriodFilter,
                this.maxPeriodIntervalFilter == null ? this.maxPeriodIntervalFilterEmpty : this.maxPeriodIntervalFilter,
                this.minPeriodIntervalFilter == null ? this.minPeriodIntervalFilterEmpty : this.minPeriodIntervalFilter,
                this.userNameFilter,
                this.userName2Filter,
                this.severityNameFilter,
                this.taskStatusNameFilter,
                this.selectedDisplayFilter == 1,
                this.selectedDisplayFilter == 2,
                this.selectedDisplayFilter == 3,
                this.selectedDisplayFilter == 4,
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

    createPaxTask(): void {
        this._router.navigate(['app/main/taskManager/paxTasks/details']);
    }

    showHistory(paxTask: PaxTaskDto): void {
        this.entityTypeHistoryModal.show({
            entityId: paxTask.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: '',
        });
    }

    // deletePaxTask(paxTask: PaxTaskDto): void {
    //     this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
    //         if (isConfirmed) {
    //             this._paxTasksServiceProxy.delete(paxTask.id).subscribe(() => {
    //                 this.reloadPage();
    //                 this.notify.success(this.l('SuccessfullyDeleted'));
    //             });
    //         }
    //     });
    // }

    exportToExcel(): void {
        this._paxTasksServiceProxy
            .getPaxTasksToExcel(
                this.filterText,
                this.headerFilter,
                this.maxCreatedDateFilter === undefined
                    ? this.maxCreatedDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxCreatedDateFilter),
                this.minCreatedDateFilter === undefined
                    ? this.minCreatedDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minCreatedDateFilter),
                this.taskTypeFilter,
                this.taskTypePeriodFilter,
                this.maxPeriodIntervalFilter == null ? this.maxPeriodIntervalFilterEmpty : this.maxPeriodIntervalFilter,
                this.minPeriodIntervalFilter == null ? this.minPeriodIntervalFilterEmpty : this.minPeriodIntervalFilter,
                this.userNameFilter,
                this.userName2Filter,
                this.severityNameFilter,
                this.taskStatusNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    showDetails(taskId: number): void {
        this._router.navigate(['app/main/taskManager/paxTasks/details'], {
            queryParams: {
                taskId: taskId
            }
        });
    }
}
