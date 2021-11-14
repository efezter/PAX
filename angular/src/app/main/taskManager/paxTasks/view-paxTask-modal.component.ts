import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetPaxTaskForViewDto, PaxTaskDto, TaskType, TaskTypePeriod } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewPaxTaskModal',
    templateUrl: './view-paxTask-modal.component.html',
})
export class ViewPaxTaskModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetPaxTaskForViewDto;
    taskType = TaskType;
    taskTypePeriod = TaskTypePeriod;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetPaxTaskForViewDto();
        this.item.paxTask = new PaxTaskDto();
    }

    show(item: GetPaxTaskForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
