import { Component, ViewChild, Injector, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { PaxTasksServiceProxy, PaxTaskUserLookupTableDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
@Component({
    selector: 'paxTaskUserLookupTableModal',
    styleUrls: ['./paxTask-user-lookup-table-modal.component.less'],
    encapsulation: ViewEncapsulation.None,
    templateUrl: './paxTask-user-lookup-table-modal.component.html',
})
export class PaxTaskUserLookupTableModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    filterText = '';
    id: number;
    displayName: string;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    active = false;
    saving = false;

    constructor(injector: Injector, private _paxTasksServiceProxy: PaxTasksServiceProxy) {
        super(injector);
    }

    show(): void {
       
        this.active = true;
        this.paginator.rows = 5;
        this.getAll();
        this.modal.show();
    }

    getAll(event?: LazyLoadEvent) {
        if (!this.active) {
            return;
        }
        // if (this.primengTableHelper.shouldResetPaging(event)) {
        //     this.paginator.changePage(0);
        //     return;
        // }

        this.primengTableHelper.showLoadingIndicator();

        let omitUserIds = new Array<number>();

        if(this.id)
            omitUserIds.push(this.id);

        this._paxTasksServiceProxy
            .getAllUserForLookupTable(
                this.filterText,
                omitUserIds,
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
        debugger;
        this.paginator.changePage(this.paginator.getPage());
    }

    setAndSave(user: PaxTaskUserLookupTableDto) {
        this.id = user.userId;
        this.displayName = user.displayName;
        this.active = false;
        this.modal.hide();
        this.modalSave.emit(null);
    }

    close(): void {
        this.active = false;
        this.modal.hide();
        this.modalSave.emit(null);
    }
}
