import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { finalize } from 'rxjs/operators';
import {
    PaxTasksServiceProxy,
    TaskDependancyRelationsServiceProxy,
    CommentsServiceProxy,
    CreateOrEditPaxTaskDto,
    PaxTaskSeverityLookupTableDto,
    PaxTaskTaskStatusLookupTableDto,
    TaskType,
    CreateOrEditCommentDto,
    GetCommentForViewDto,
    WatcherUserLookupTableDto,
    CommentDto,
    PaxTaskAttachmentsServiceProxy,
    PaxTaskAttachmentDto,
    PaxTaskUserLookupTableDto,
    HistoryDto,
    LabelDto,
    TaskDependancyRelationDto
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { AbpSessionService } from 'abp-ng2-module';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { PaxTaskUserLookupTableModalComponent } from './paxTask-user-lookup-table-modal.component';
import { ActivatedRoute, Router } from '@angular/router';
import { AppConsts } from '@shared/AppConsts';
import { appModuleAnimation } from '@shared/animations/routerTransition';
// import { LazyLoadEvent } from 'primeng/api';
// import { ChangeEvent } from '@ckeditor/ckeditor5-angular/ckeditor.component';
// import Mention from '@ckeditor/ckeditor5-mention/src/mention';
// import { add } from 'lodash';

import * as CustomCK from 'shared/customCK/build/ckeditor';
import { filter } from 'lodash-es';


@Component({
    selector: 'createOrEditPaxTaskModal',
    templateUrl: './create-or-edit-paxTask.component.html',
    styleUrls: ['./create-or-edit-paxTask.component.less'],
    animations: [appModuleAnimation()]
})
export class CreateOrEditPaxTaskModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('paxTaskUserLookupTableModal', { static: true })
    paxTaskUserLookupTableModal: PaxTaskUserLookupTableModalComponent;

    public Editor = CustomCK;
    public Editorr = CustomCK;
    public Editorrr = CustomCK;
    public commentEditors: any[] = new Array<{ commentId: number, editor: any }>();
  
    watchers: WatcherUserLookupTableDto[] = new Array<WatcherUserLookupTableDto>();
    filteredWathers: PaxTaskUserLookupTableDto[];

    labels:LabelDto[] = new Array<LabelDto>();
    filteredLabels: LabelDto[];

    depentantTasks:TaskDependancyRelationDto[] = new Array<TaskDependancyRelationDto>();
    filteredDependants: TaskDependancyRelationDto[];
    
    
    historyRecs: HistoryDto[] = new Array<HistoryDto>();
    
    commentToCreateOrEdit: CreateOrEditCommentDto = new CreateOrEditCommentDto();
    isCommentEditing: boolean = false;

    uploadUrl: string;
    uploadedFiles: PaxTaskAttachmentDto[] = [];

    deadLineDate:Date;


       // Create a configuration object
editorConfiguration = {
    mention: {
        feeds: []
    }
};

    active = false;
    saving = false;
    minDateValue:Date = new Date();

    paxTask: CreateOrEditPaxTaskDto = new CreateOrEditPaxTaskDto();
    commentViews: GetCommentForViewDto[];
    showCommentCreate: boolean = false;

    cuRuserId = -1;
    reporterName = '';
    assigneeName = '';
    severityName = '';
    taskStatusName = '';
    serverUrl='';
    clientUrl='';

    selectedBGColors:any[] = [
    "#3485fd",
    "#7e36f4",
    "#8660cb",
    "#dd5498",
    "#e25563",
    "#fd933a",
    "#ffcb2f",
    "#4ab563",
    "#44d2a8",
    "#61c0cf",
    "#6681D5",
    "#638BD1",
    "#5AA7C5",
    "#54BABD",
    "#53BFBB",
    "#51C3B9",
    "#A26BB7",
    "#8476C6",
    "#757CCE",
    "#D65276",
    "#C94E88",
    "#BD7397",
    "#D28777",
    "#E48D5C",
    "#D76E72"];   

    allSeveritys: PaxTaskSeverityLookupTableDto[];
    allTaskStatuss: PaxTaskTaskStatusLookupTableDto[];
    selectedStatus: PaxTaskTaskStatusLookupTableDto;


    filterCountry(event) {
        //in a real application, make a request to a remote url with the query and return filtered results, for demo we filter at client side
        let filtered: any[] = [];
        let query = event.query;
        for (let i = 0; i < this.countries.length; i++) {
          let country = this.countries[i];
          if (country.name.toLowerCase().indexOf(query.toLowerCase()) == 0) {
            filtered.push(country);
          }
        }
    
        this.filteredCountries = filtered;
      }


    filteredCountries: any[];
    selectedCountries: any[];


    countries: any[] =  [ 
            {"name": "Afghanistan", "code": "AF"}, 
            {"name": "Åland Islands", "code": "AX"}, 
            {"name": "Albania", "code": "AL"}, 
            {"name": "Algeria", "code": "DZ"}, 
            {"name": "American Samoa", "code": "AS"}, 
            {"name": "Andorra", "code": "AD"}, 
            {"name": "Angola", "code": "AO"}, 
            {"name": "Anguilla", "code": "AI"}, 
            {"name": "Antarctica", "code": "AQ"}, 
            {"name": "Antigua and Barbuda", "code": "AG"}, 
            {"name": "Argentina", "code": "AR"}, 
            {"name": "Armenia", "code": "AM"}, 
            {"name": "Aruba", "code": "AW"}, 
            {"name": "Australia", "code": "AU"}, 
            {"name": "Austria", "code": "AT"}, 
            {"name": "Azerbaijan", "code": "AZ"}, 
            {"name": "Bahamas", "code": "BS"}, 
            {"name": "Bahrain", "code": "BH"}, 
            {"name": "Bangladesh", "code": "BD"}, 
            {"name": "Barbados", "code": "BB"}, 
            {"name": "Belarus", "code": "BY"}, 
            {"name": "Belgium", "code": "BE"}, 
            {"name": "Belize", "code": "BZ"}, 
            {"name": "Benin", "code": "BJ"}, 
            {"name": "Bermuda", "code": "BM"}, 
            {"name": "Bhutan", "code": "BT"}, 
            {"name": "Bolivia", "code": "BO"}, 
            {"name": "Bosnia and Herzegovina", "code": "BA"}, 
            {"name": "Botswana", "code": "BW"}, 
            {"name": "Bouvet Island", "code": "BV"}, 
            {"name": "Brazil", "code": "BR"}, 
            {"name": "British Indian Ocean Territory", "code": "IO"}, 
            {"name": "Brunei Darussalam", "code": "BN"}, 
            {"name": "Bulgaria", "code": "BG"}, 
            {"name": "Burkina Faso", "code": "BF"}, 
            {"name": "Burundi", "code": "BI"}, 
            {"name": "Cambodia", "code": "KH"}, 
            {"name": "Cameroon", "code": "CM"}, 
            {"name": "Canada", "code": "CA"}, 
            {"name": "Cape Verde", "code": "CV"}, 
            {"name": "Cayman Islands", "code": "KY"}, 
            {"name": "Central African Republic", "code": "CF"}, 
            {"name": "Chad", "code": "TD"}, 
            {"name": "Chile", "code": "CL"}, 
            {"name": "China", "code": "CN"}, 
            {"name": "Christmas Island", "code": "CX"}, 
            {"name": "Cocos (Keeling) Islands", "code": "CC"}, 
            {"name": "Colombia", "code": "CO"}, 
            {"name": "Comoros", "code": "KM"}, 
            {"name": "Congo", "code": "CG"}, 
            {"name": "Congo, The Democratic Republic of the", "code": "CD"}, 
            {"name": "Cook Islands", "code": "CK"}, 
            {"name": "Costa Rica", "code": "CR"}, 
            {"name": "Cote D\"Ivoire", "code": "CI"}, 
            {"name": "Croatia", "code": "HR"}, 
            {"name": "Cuba", "code": "CU"}, 
            {"name": "Cyprus", "code": "CY"}, 
            {"name": "Czech Republic", "code": "CZ"}, 
            {"name": "Denmark", "code": "DK"}, 
            {"name": "Djibouti", "code": "DJ"}, 
            {"name": "Dominica", "code": "DM"}, 
            {"name": "Dominican Republic", "code": "DO"}, 
            {"name": "Ecuador", "code": "EC"}, 
            {"name": "Egypt", "code": "EG"}, 
            {"name": "El Salvador", "code": "SV"}, 
            {"name": "Equatorial Guinea", "code": "GQ"}, 
            {"name": "Eritrea", "code": "ER"}, 
            {"name": "Estonia", "code": "EE"}, 
            {"name": "Ethiopia", "code": "ET"}, 
            {"name": "Falkland Islands (Malvinas)", "code": "FK"}, 
            {"name": "Faroe Islands", "code": "FO"}, 
            {"name": "Fiji", "code": "FJ"}, 
            {"name": "Finland", "code": "FI"}, 
            {"name": "France", "code": "FR"}, 
            {"name": "French Guiana", "code": "GF"}, 
            {"name": "French Polynesia", "code": "PF"}, 
            {"name": "French Southern Territories", "code": "TF"}, 
            {"name": "Gabon", "code": "GA"}, 
            {"name": "Gambia", "code": "GM"}, 
            {"name": "Georgia", "code": "GE"}, 
            {"name": "Germany", "code": "DE"}, 
            {"name": "Ghana", "code": "GH"}, 
            {"name": "Gibraltar", "code": "GI"}, 
            {"name": "Greece", "code": "GR"}, 
            {"name": "Greenland", "code": "GL"}, 
            {"name": "Grenada", "code": "GD"}, 
            {"name": "Guadeloupe", "code": "GP"}, 
            {"name": "Guam", "code": "GU"}, 
            {"name": "Guatemala", "code": "GT"}, 
            {"name": "Guernsey", "code": "GG"}, 
            {"name": "Guinea", "code": "GN"}, 
            {"name": "Guinea-Bissau", "code": "GW"}, 
            {"name": "Guyana", "code": "GY"}, 
            {"name": "Haiti", "code": "HT"}, 
            {"name": "Heard Island and Mcdonald Islands", "code": "HM"}, 
            {"name": "Holy See (Vatican City State)", "code": "VA"}, 
            {"name": "Honduras", "code": "HN"}, 
            {"name": "Hong Kong", "code": "HK"}, 
            {"name": "Hungary", "code": "HU"}, 
            {"name": "Iceland", "code": "IS"}, 
            {"name": "India", "code": "IN"}, 
            {"name": "Indonesia", "code": "ID"}, 
            {"name": "Iran, Islamic Republic Of", "code": "IR"}, 
            {"name": "Iraq", "code": "IQ"}, 
            {"name": "Ireland", "code": "IE"}, 
            {"name": "Isle of Man", "code": "IM"}, 
            {"name": "Israel", "code": "IL"}, 
            {"name": "Italy", "code": "IT"}, 
            {"name": "Jamaica", "code": "JM"}, 
            {"name": "Japan", "code": "JP"}, 
            {"name": "Jersey", "code": "JE"}, 
            {"name": "Jordan", "code": "JO"}, 
            {"name": "Kazakhstan", "code": "KZ"}, 
            {"name": "Kenya", "code": "KE"}, 
            {"name": "Kiribati", "code": "KI"}, 
            {"name": "Korea, Democratic People\"S Republic of", "code": "KP"}, 
            {"name": "Korea, Republic of", "code": "KR"}, 
            {"name": "Kuwait", "code": "KW"}, 
            {"name": "Kyrgyzstan", "code": "KG"}, 
            {"name": "Lao People\"S Democratic Republic", "code": "LA"}, 
            {"name": "Latvia", "code": "LV"}, 
            {"name": "Lebanon", "code": "LB"}, 
            {"name": "Lesotho", "code": "LS"}, 
            {"name": "Liberia", "code": "LR"}, 
            {"name": "Libyan Arab Jamahiriya", "code": "LY"}, 
            {"name": "Liechtenstein", "code": "LI"}, 
            {"name": "Lithuania", "code": "LT"}, 
            {"name": "Luxembourg", "code": "LU"}, 
            {"name": "Macao", "code": "MO"}, 
            {"name": "Macedonia, The Former Yugoslav Republic of", "code": "MK"}, 
            {"name": "Madagascar", "code": "MG"}, 
            {"name": "Malawi", "code": "MW"}, 
            {"name": "Malaysia", "code": "MY"}, 
            {"name": "Maldives", "code": "MV"}, 
            {"name": "Mali", "code": "ML"}, 
            {"name": "Malta", "code": "MT"}, 
            {"name": "Marshall Islands", "code": "MH"}, 
            {"name": "Martinique", "code": "MQ"}, 
            {"name": "Mauritania", "code": "MR"}, 
            {"name": "Mauritius", "code": "MU"}, 
            {"name": "Mayotte", "code": "YT"}, 
            {"name": "Mexico", "code": "MX"}, 
            {"name": "Micronesia, Federated States of", "code": "FM"}, 
            {"name": "Moldova, Republic of", "code": "MD"}, 
            {"name": "Monaco", "code": "MC"}, 
            {"name": "Mongolia", "code": "MN"}, 
            {"name": "Montserrat", "code": "MS"}, 
            {"name": "Morocco", "code": "MA"}, 
            {"name": "Mozambique", "code": "MZ"}, 
            {"name": "Myanmar", "code": "MM"}, 
            {"name": "Namibia", "code": "NA"}, 
            {"name": "Nauru", "code": "NR"}, 
            {"name": "Nepal", "code": "NP"}, 
            {"name": "Netherlands", "code": "NL"}, 
            {"name": "Netherlands Antilles", "code": "AN"}, 
            {"name": "New Caledonia", "code": "NC"}, 
            {"name": "New Zealand", "code": "NZ"}, 
            {"name": "Nicaragua", "code": "NI"}, 
            {"name": "Niger", "code": "NE"}, 
            {"name": "Nigeria", "code": "NG"}, 
            {"name": "Niue", "code": "NU"}, 
            {"name": "Norfolk Island", "code": "NF"}, 
            {"name": "Northern Mariana Islands", "code": "MP"}, 
            {"name": "Norway", "code": "NO"}, 
            {"name": "Oman", "code": "OM"}, 
            {"name": "Pakistan", "code": "PK"}, 
            {"name": "Palau", "code": "PW"}, 
            {"name": "Palestinian Territory, Occupied", "code": "PS"}, 
            {"name": "Panama", "code": "PA"}, 
            {"name": "Papua New Guinea", "code": "PG"}, 
            {"name": "Paraguay", "code": "PY"}, 
            {"name": "Peru", "code": "PE"}, 
            {"name": "Philippines", "code": "PH"}, 
            {"name": "Pitcairn", "code": "PN"}, 
            {"name": "Poland", "code": "PL"}, 
            {"name": "Portugal", "code": "PT"}, 
            {"name": "Puerto Rico", "code": "PR"}, 
            {"name": "Qatar", "code": "QA"}, 
            {"name": "Reunion", "code": "RE"}, 
            {"name": "Romania", "code": "RO"}, 
            {"name": "Russian Federation", "code": "RU"}, 
            {"name": "RWANDA", "code": "RW"}, 
            {"name": "Saint Helena", "code": "SH"}, 
            {"name": "Saint Kitts and Nevis", "code": "KN"}, 
            {"name": "Saint Lucia", "code": "LC"}, 
            {"name": "Saint Pierre and Miquelon", "code": "PM"}, 
            {"name": "Saint Vincent and the Grenadines", "code": "VC"}, 
            {"name": "Samoa", "code": "WS"}, 
            {"name": "San Marino", "code": "SM"}, 
            {"name": "Sao Tome and Principe", "code": "ST"}, 
            {"name": "Saudi Arabia", "code": "SA"}, 
            {"name": "Senegal", "code": "SN"}, 
            {"name": "Serbia and Montenegro", "code": "CS"}, 
            {"name": "Seychelles", "code": "SC"}, 
            {"name": "Sierra Leone", "code": "SL"}, 
            {"name": "Singapore", "code": "SG"}, 
            {"name": "Slovakia", "code": "SK"}, 
            {"name": "Slovenia", "code": "SI"}, 
            {"name": "Solomon Islands", "code": "SB"}, 
            {"name": "Somalia", "code": "SO"}, 
            {"name": "South Africa", "code": "ZA"}, 
            {"name": "South Georgia and the South Sandwich Islands", "code": "GS"}, 
            {"name": "Spain", "code": "ES"}, 
            {"name": "Sri Lanka", "code": "LK"}, 
            {"name": "Sudan", "code": "SD"}, 
            {"name": "Suriname", "code": "SR"}, 
            {"name": "Svalbard and Jan Mayen", "code": "SJ"}, 
            {"name": "Swaziland", "code": "SZ"}, 
            {"name": "Sweden", "code": "SE"}, 
            {"name": "Switzerland", "code": "CH"}, 
            {"name": "Syrian Arab Republic", "code": "SY"}, 
            {"name": "Taiwan, Republic of China", "code": "TW"}, 
            {"name": "Tajikistan", "code": "TJ"}, 
            {"name": "Tanzania, United Republic of", "code": "TZ"}, 
            {"name": "Thailand", "code": "TH"}, 
            {"name": "Timor-Leste", "code": "TL"}, 
            {"name": "Togo", "code": "TG"}, 
            {"name": "Tokelau", "code": "TK"}, 
            {"name": "Tonga", "code": "TO"}, 
            {"name": "Trinidad and Tobago", "code": "TT"}, 
            {"name": "Tunisia", "code": "TN"}, 
            {"name": "Turkey", "code": "TR"}, 
            {"name": "Turkmenistan", "code": "TM"}, 
            {"name": "Turks and Caicos Islands", "code": "TC"}, 
            {"name": "Tuvalu", "code": "TV"}, 
            {"name": "Uganda", "code": "UG"}, 
            {"name": "Ukraine", "code": "UA"}, 
            {"name": "United Arab Emirates", "code": "AE"}, 
            {"name": "United Kingdom", "code": "GB"}, 
            {"name": "United States", "code": "US"}, 
            {"name": "United States Minor Outlying Islands", "code": "UM"}, 
            {"name": "Uruguay", "code": "UY"}, 
            {"name": "Uzbekistan", "code": "UZ"}, 
            {"name": "Vanuatu", "code": "VU"}, 
            {"name": "Venezuela", "code": "VE"}, 
            {"name": "Viet Nam", "code": "VN"}, 
            {"name": "Virgin Islands, British", "code": "VG"}, 
            {"name": "Virgin Islands, U.S.", "code": "VI"}, 
            {"name": "Wallis and Futuna", "code": "WF"}, 
            {"name": "Western Sahara", "code": "EH"}, 
            {"name": "Yemen", "code": "YE"}, 
            {"name": "Zambia", "code": "ZM"}, 
            {"name": "Zimbabwe", "code": "ZW"} 
        ]
    








    constructor(
        injector: Injector,
        private _paxTasksServiceProxy: PaxTasksServiceProxy,
        private _taskDependancyRelationsServiceProxy: TaskDependancyRelationsServiceProxy,
        private _paxTaskAttachmentsServiceProxy: PaxTaskAttachmentsServiceProxy,
        private _commentsServiceProxy: CommentsServiceProxy,
        // private _dateTimeService: DateTimeService,
        private _activatedRoute: ActivatedRoute,
        private _router: Router,
        private _abpSessionService: AbpSessionService,
        private elem: ElementRef
    ) {
        super(injector);
        this.commentToCreateOrEdit.commentText = "";
        this.editorConfiguration.mention.feeds.push( {
            marker: '@',
            feed: this.getFeedUsers,
            itemRenderer: this.customItemRenderer,
            minimumCharacters: 3
        });
    }

    onSelect(event: any){
        setTimeout(() => {this.generateBGColor()}, 50); 
      }

      generateBGColor()
      {
        let elements = this.elem.nativeElement.querySelectorAll('.p-autocomplete-token');
        elements.forEach(el => {
            if (!el.getAttribute('colored')) {
                el.style.backgroundColor = this.selectedBGColors[Math.floor(Math.random() * (24 - 0 + 1) + 0)]; + " !important";
                el.setAttribute('colored', 'true')
            }            
        });
      }

    customItemRenderer( item ) {
        const itemElement = document.createElement( 'div' );
    
        itemElement.classList.add( 'mention-item' );
        itemElement.id = `mention-list-item-id-${ item.userId }`;
        itemElement.textContent = `${ item.id } `;

        return itemElement;
    }

    getFeedUsers( queryText ) {   
       return this._paxTasksServiceProxy
        .getUsersForMentionCustom(
            queryText
        );       
    }

    getFeedAttachments( queryText ) {        
        return this.uploadedFiles.filter(x => x.fileName.includes(queryText));
    }

    // public onChange( { editor }: ChangeEvent ) {

    //     // const data = editor.getData();
    //     editor.ui.view.toolbar.element.style.display = 'none';
    //     // console.log( data );
    // }
    

    ngOnInit(): void {
        this.selectedStatus = new PaxTaskTaskStatusLookupTableDto();

        this.cuRuserId = this._abpSessionService.userId;
        this.serverUrl = AppConsts.remoteServiceBaseUrl;
        this.clientUrl = AppConsts.appBaseUrl;
        let paxTaskId = parseInt(this._activatedRoute.snapshot.queryParams['taskId']);

        this.commentViews = new Array<GetCommentForViewDto>();
        this.uploadedFiles = new Array<PaxTaskAttachmentDto>();

        if (!paxTaskId) {
            this.paxTask = new CreateOrEditPaxTaskDto();
            this.paxTask.id = paxTaskId;
            this.paxTask.taskType = TaskType.Normal;
            this.reporterName = '';
            this.assigneeName = '';
            this.severityName = '';
            this.taskStatusName = '';
            this.active = true;
            this.getAllTaskStatuses();
        } else {
            this.uploadUrl = this.serverUrl + '/PaxTask/UploadFiles?taskId=' + paxTaskId;
            this.getAttachments(paxTaskId); 
            this.getTaskDetails(paxTaskId);            
        }

        this._paxTasksServiceProxy.getAllSeverityForTableDropdown().subscribe((result) => {
            this.allSeveritys = result;
        });       
    }

    getAllTaskStatuses()
    {
        this._paxTasksServiceProxy.getAllTaskStatusForTableDropdown().subscribe((result) => {
            this.allTaskStatuss = result;
            if (this.paxTask.id)
            {
                this.selectedStatus = this.allTaskStatuss.find(x => x.id == this.paxTask.taskStatusId);
            }   
            else
            {
                this.selectedStatus = this.allTaskStatuss[0];
            }    
        });
    }

    ngAfterViewInit(): void{
        setTimeout(() => {this.generateBGColor()}, 2500); 
    }

    public onReady(ckEditor, commentId) {

        ckEditor._paxTasksServiceProxy = this._paxTasksServiceProxy;
        
        if (commentId != 0) {//Edit editors
            ckEditor.ui.view.toolbar.element.style.display = 'none';
            ckEditor.isReadOnly = true;
            this.commentEditors.push({ commentId: commentId, editor: ckEditor });
        }        
    }

    public prepareForEdit(commentId: number, toggle: boolean) {

        let editor = this.commentEditors.find(e => e.commentId == commentId).editor;

        if (!toggle) {
            editor.ui.view.toolbar.element.style.display = 'none';
            editor.isReadOnly = true;
            this.isCommentEditing = false;
        } else {
            editor.ui.view.toolbar.element.style.display = 'flex';
            editor.isReadOnly = false;
            this.isCommentEditing = true;
            if (this.showCommentCreate == true) {
                this.showCommentCreate = false;
            }
        }
    }

    getUsers(event) {
        this._paxTasksServiceProxy
            .getAllUserForLookupTable(
                event.query,
                this.watchers.map(a => a.userId),
                undefined,
                undefined,
                100                
            )
            .subscribe((result) => {
                this.filteredWathers = result.items;
            });
    }

    getLabels(event) {

        let labelIds = []
        
        if (this.labels && this.labels.length > 0) {
            labelIds = this.labels.map(a => a.id);
        }        

        this._paxTasksServiceProxy
            .getTaskLabels(
                event.query,
                labelIds,
                undefined,
                undefined,
                100                
            )
            .subscribe((result) => {
                this.filteredLabels = result;
            });
    }

    getHistory(paxTaskId: number) {
        this._paxTasksServiceProxy
            .getTaskHistory(paxTaskId)
            .subscribe((result) => {
                this.historyRecs = result.items;
            });
    }

    filterDependantTasks(event) {
        this._paxTasksServiceProxy
        .getTasksForDepandancyDrop(event.query,undefined)
            .subscribe((result) => {

                this.filteredDependants = result;
            });
    }

    getAttachments(paxTaskId: number) {

        this._paxTaskAttachmentsServiceProxy
            .getAll(
                    undefined,
                    paxTaskId,
                    undefined,
                    undefined,
                    undefined,
                    undefined,
                    25
            )
            .subscribe((result) => {
                this.uploadedFiles = result.items;

                this.getHistory(paxTaskId);

                this.editorConfiguration.mention.feeds.push({
                    marker: '#',
                    feed: this.uploadedFiles.map(x => "#" + x.fileName),
                    itemRenderer: this.customItemRenderer,
                    minimumCharacters: 1
                });
            });
    }

    deleteAttachment(attachmenttId: number): void {

        this.message.confirm(
            this.l('DeleteAttachmentMessage'),
            this.l('AreYouSure'),
            isConfirmed => {
                if (isConfirmed) {
                    this.saving = true;

                    this._paxTaskAttachmentsServiceProxy
                        .delete(attachmenttId)
                        .pipe(
                            finalize(() => {
                                this.saving = false;
                            })
                        )
                        .subscribe((res) => {
                            let deletedItem = this.uploadedFiles.find(e => e.id == attachmenttId);

                            this.uploadedFiles.splice(this.uploadedFiles.indexOf(deletedItem), 1)

                            this.notify.info(this.l('SavedSuccessfully'));
                        });
                }
            }
        );
    }

    save(): void {

        this.saving = true;

        this.paxTask.watchers = this.watchers;
        this.paxTask.labels = this.labels;
        this.paxTask.dependentTasks = this.depentantTasks;
        this.paxTask.taskStatusId = this.selectedStatus.id;

        this.paxTask.deadLineDate = DateTime.fromJSDate(this.deadLineDate);

        this._paxTasksServiceProxy
            .createOrEdit(this.paxTask)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((res) => {
                this.notify.info(this.l('SavedSuccessfully'));

                if (!this.paxTask.id) {
                    this._router.navigate(['app/main/taskManager/paxTasks/details'], {
                        queryParams: {
                            taskId: res.value
                        }
                    }).then(() => {window.location.reload()});
                }
            });
    }

    openSelectUserModal() {
        this.paxTaskUserLookupTableModal.id = this.paxTask.assigneeId;
        this.paxTaskUserLookupTableModal.displayName = this.assigneeName;
        this.paxTaskUserLookupTableModal.show();
    }

    setAssigneeIdNull() {
        this.paxTask.assigneeId = null;
        this.assigneeName = '';
    }

    getNewAssigneeId() {
        this.paxTask.assigneeId = this.paxTaskUserLookupTableModal.id;
        this.assigneeName = this.paxTaskUserLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this._router.navigate(['app/main/taskManager/paxTasks']);
    }

    getTaskDetails(paxTaskId: number): void {

        this._paxTasksServiceProxy.getPaxTaskForEdit(paxTaskId).subscribe((result) => {
            this.paxTask = result.paxTask;
            this.watchers = result.paxTask.watchers;
            this.labels = result.paxTask.labels;
            this.depentantTasks = result.paxTask.dependentTasks;
            this.deadLineDate = result.paxTask.deadLineDate.toJSDate();

            this.reporterName = result.userName;
            this.assigneeName = result.userName2;
            this.severityName = result.severityName;
            this.taskStatusName = result.taskStatusName;    
            this.getTaskComments();
            this.getAllTaskStatuses();
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

    toggleCommentCreate(): void {
        this.showCommentCreate = !this.showCommentCreate;
    }

    saveComment(comment: CommentDto): void {

        this.saving = true;

        if (!comment) {
            this.commentToCreateOrEdit.paxTaskId = this.paxTask.id;
        }
        else {
            this.commentToCreateOrEdit.paxTaskId = this.paxTask.id;
            this.commentToCreateOrEdit.id = comment.id;
            this.commentToCreateOrEdit.commentText = comment.commentText;
        }

        this._commentsServiceProxy
            .createOrEdit(this.commentToCreateOrEdit)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((res) => {

                if (!comment) {

                    let added:GetCommentForViewDto = new GetCommentForViewDto();

                    added.creationTime = DateTime.now();
                    added.comment = new CommentDto();
                    added.comment.id = res.id;
                    added.comment.commentText = res.commentText;
                    added.comment.userId = this._abpSessionService.userId;
                    this.commentViews.unshift(added)

                    this.showCommentCreate = false;
                    this.commentToCreateOrEdit = new CreateOrEditCommentDto();
                    this.commentToCreateOrEdit.commentText = "";

                } else {
                    this.prepareForEdit(comment.id, false);
                }

                this.notify.info(this.l('SavedSuccessfully'));
            });
    }

    deleteComment(commentId: number): void {

        this.message.confirm(
            this.l('DeleteCommentMessage'),
            this.l('AreYouSure'),
            isConfirmed => {
                if (isConfirmed) {
                    this.saving = true;

                    this._commentsServiceProxy
                        .delete(commentId)
                        .pipe(
                            finalize(() => {
                                this.saving = false;
                            })
                        )
                        .subscribe((res) => {

                            let deletedItem = this.commentViews.find(e => e.comment.id == commentId);

                            this.commentViews.splice(this.commentViews.indexOf(deletedItem), 1)

                            this.notify.info(this.l('SavedSuccessfully'));
                        });
                }
            }
        );
    }

      // upload completed event
      onUpload(event): void {
        //   event.originalEvent.body.result
        for (const file of event.originalEvent.body.result) {
        
            var savedAtch:PaxTaskAttachmentDto = new PaxTaskAttachmentDto();
            savedAtch.fileName = file.fileName;
            savedAtch.fileUrl = file.fileUrl;
            savedAtch.id = file.id;
            savedAtch.creationTime = file.creationTime ? DateTime.fromISO(file.creationTime.toString()) : <any>undefined;
            savedAtch.userName = file.userName;
            this.uploadedFiles.push(savedAtch);
        }
    }

    onBeforeSend(event): void {
        event.xhr.setRequestHeader('Authorization', 'Bearer ' + abp.auth.getToken());
    }
}
