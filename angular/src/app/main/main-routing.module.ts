<<<<<<< Updated upstream
﻿import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    
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
=======
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
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
>>>>>>> Stashed changes
