import { browser, by, element } from 'protractor';

// ReSharper disable TsResolvedFromInaccessibleModule
export class AppPage {
  navigateTo() {
    return browser.get('/');
  }

  getMainHeading() {
    return element(by.css('app-root h1')).getText();
  }
}
// ReSharper restore TsResolvedFromInaccessibleModule
