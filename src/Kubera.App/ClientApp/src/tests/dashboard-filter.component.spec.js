"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
/// <reference path="../../node_modules/@types/jasmine/index.d.ts" />
var testing_1 = require("@angular/core/testing");
var platform_browser_1 = require("@angular/platform-browser");
var dashboard_filter_component_1 = require("../app/dashboard/dashboard-filter/dashboard-filter.component");
var component;
var fixture;
describe('dashboard-filter component', function () {
    beforeEach(testing_1.waitForAsync(function () {
        testing_1.TestBed.configureTestingModule({
            declarations: [dashboard_filter_component_1.DashboardFilterComponent],
            imports: [platform_browser_1.BrowserModule],
            providers: [
                { provide: testing_1.ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = testing_1.TestBed.createComponent(dashboard_filter_component_1.DashboardFilterComponent);
        component = fixture.componentInstance;
    }));
    it('should do something', testing_1.waitForAsync(function () {
        expect(true).toEqual(true);
    }));
});
//# sourceMappingURL=dashboard-filter.component.spec.js.map