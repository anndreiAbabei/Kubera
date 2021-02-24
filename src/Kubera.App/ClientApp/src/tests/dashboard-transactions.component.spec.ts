/// <reference path="../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, ComponentFixture, ComponentFixtureAutoDetect, waitForAsync } from '@angular/core/testing';
import { BrowserModule } from '@angular/platform-browser';
import { DashboardTransactionsComponent } from '../app/dashboard/dashboard-transactions/dashboard-transactions.component';

let component: DashboardTransactionsComponent;
let fixture: ComponentFixture<DashboardTransactionsComponent>;

describe('dashboard-transactions component', () => {
    beforeEach(waitForAsync(() => {
        TestBed.configureTestingModule({
            declarations: [ DashboardTransactionsComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(DashboardTransactionsComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', waitForAsync(() => {
        expect(true).toEqual(true);
    }));
});
