import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SeveritiesServiceProxy, SeverityDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditSeverityModalComponent } from './create-or-edit-severity-modal.component';

import { ViewSeverityModalComponent } from './view-severity-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './severities.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class SeveritiesComponent extends AppComponentBase {
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('createOrEditSeverityModal', { static: true })
    createOrEditSeverityModal: CreateOrEditSeverityModalComponent;
    @ViewChild('viewSeverityModalComponent', { static: true }) viewSeverityModal: ViewSeverityModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    maxOrderFilter: number;
    maxOrderFilterEmpty: number;
    minOrderFilter: number;
    minOrderFilterEmpty: number;
    maxInsertedDateFilter: DateTime;
    minInsertedDateFilter: DateTime;

    _entityTypeFullName = 'PAX.Next.TaskManager.Severity';
    entityHistoryEnabled = false;

    constructor(
        injector: Injector,
        private _severitiesServiceProxy: SeveritiesServiceProxy,
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

    getSeverities(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._severitiesServiceProxy
            .getAll(
                this.filterText,
                this.nameFilter,
                this.maxOrderFilter == null ? this.maxOrderFilterEmpty : this.maxOrderFilter,
                this.minOrderFilter == null ? this.minOrderFilterEmpty : this.minOrderFilter,
                this.maxInsertedDateFilter === undefined
                    ? this.maxInsertedDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxInsertedDateFilter),
                this.minInsertedDateFilter === undefined
                    ? this.minInsertedDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minInsertedDateFilter),
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

    createSeverity(): void {
        this.createOrEditSeverityModal.show();
    }

    showHistory(severity: SeverityDto): void {
        this.entityTypeHistoryModal.show({
            entityId: severity.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: '',
        });
    }

    deleteSeverity(severity: SeverityDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._severitiesServiceProxy.delete(severity.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._severitiesServiceProxy
            .getSeveritiesToExcel(
                this.filterText,
                this.nameFilter,
                this.maxOrderFilter == null ? this.maxOrderFilterEmpty : this.maxOrderFilter,
                this.minOrderFilter == null ? this.minOrderFilterEmpty : this.minOrderFilter,
                this.maxInsertedDateFilter === undefined
                    ? this.maxInsertedDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxInsertedDateFilter),
                this.minInsertedDateFilter === undefined
                    ? this.minInsertedDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minInsertedDateFilter)
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
