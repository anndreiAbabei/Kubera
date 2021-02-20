"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
/// <reference path="../../node_modules/@types/jasmine/index.d.ts" />
var testing_1 = require("@angular/core/testing");
var platform_browser_1 = require("@angular/platform-browser");
var home_component_1 = require("../app/home/home.component");
var component;
var fixture;
describe('home component', function () {
    beforeEach(testing_1.waitForAsync(function () {
        testing_1.TestBed.configureTestingModule({
            declarations: [home_component_1.HomeComponent],
            imports: [platform_browser_1.BrowserModule],
            providers: [
                { provide: testing_1.ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = testing_1.TestBed.createComponent(home_component_1.HomeComponent);
        component = fixture.componentInstance;
    }));
    it('should do something', testing_1.waitForAsync(function () {
        expect(true).toEqual(true);
    }));
});
//# sourceMappingURL=home.component.spec.js.map