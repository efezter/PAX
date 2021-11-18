import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    
                    {
                        path: 'taskManager/paxTasks',
                        loadChildren: () => import('./taskManager/paxTasks/paxTask.module').then(m => m.PaxTaskModule),
                        data: { permission: 'Pages.PaxTasks' }
                    },

                    {
                        path: 'taskManager/paxTasks/details',
                        loadChildren: () => import('./taskManager/paxTasks/create-or-edit-paxTask.module').then(m => m.PaxTaskDetailsModule),
                        data: { permission: 'Pages.PaxTasks.Create' }
                    }, 
                    
                    {
                        path: 'taskManager/taskStatuses',
                        loadChildren: () => import('./taskManager/taskStatuses/taskStatus.module').then(m => m.TaskStatusModule),
                        data: { permission: 'Pages.TaskStatuses' }
                    },
                
                    
                    {
                        path: 'taskManager/severities',
                        loadChildren: () => import('./taskManager/severities/severity.module').then(m => m.SeverityModule),
                        data: { permission: 'Pages.Severities' }
                    },
                
                    {
                        path: 'dashboard',
                        loadChildren: () => import('./dashboard/dashboard.module').then(m => m.DashboardModule),
                        data: { permission: 'Pages.Tenant.Dashboard' }
                    },
                    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
                    { path: '**', redirectTo: 'dashboard' }
                ]
            }
        ])
    ],
    exports: [
        RouterModule
    ]
})
export class MainRoutingModule { }

