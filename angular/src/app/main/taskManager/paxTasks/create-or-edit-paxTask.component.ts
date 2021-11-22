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
import { DemoUiComponentsServiceProxy, NameValueOfString } from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent } from 'primeng/api';
import { ChangeEvent } from '@ckeditor/ckeditor5-angular/ckeditor.component';


import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { add } from 'lodash';

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
    public Editorr = ClassicEditor;
    public Editorrr = ClassicEditor;

    filteredCountries: WatcherUserLookupTableDto[];
    countries: WatcherUserLookupTableDto[] = new Array<WatcherUserLookupTableDto>();

    commentToCreate:CreateOrEditCommentDto = new CreateOrEditCommentDto();

    active = false;
    saving = false;

    paxTask: CreateOrEditPaxTaskDto = new CreateOrEditPaxTaskDto();
    commentViews: GetCommentForViewDto[];
    showCommentCreate:boolean = false;

    cuRuserId = -1;
    reporterName = '';
    assigneeName = '';
    severityName = '';
    taskStatusName = '';

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

    public onChange( { editor }: ChangeEvent ) {

        // const data = editor.getData();
        editor.ui.view.toolbar.element.style.display = 'none';
        // console.log( data );
    }

    public onReady( editor ) {
        editor.ui.view.toolbar.element.style.display = 'none';
        editor.isReadOnly = true;
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

    getTaskDetails(paxTaskId:number): void {

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

    toggleCommentCreate(): void
    {
           this.showCommentCreate = !this.showCommentCreate; 
    }

    saveComment(): void {
        
        this.saving = true;

        this.commentToCreate.paxTaskId = this.paxTask.id;

        this._commentsServiceProxy
            .createOrEdit(this.commentToCreate)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((res) => {               
                
                let added = new GetCommentForViewDto();

                added.creationTime = DateTime.now();
                added.comment = new CommentDto();
                added.comment.id = res.id;
                added.comment.commentText = res.commentText;

                this.commentViews.unshift(added)

                this.showCommentCreate = false;
                this.commentToCreate = new CreateOrEditCommentDto();
                this.commentToCreate.commentText = "";
                this.notify.info(this.l('SavedSuccessfully'));
            });
    }
}
