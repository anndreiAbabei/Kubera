import { AppPage } from './app.po';

// ReSharper disable TsNotResolved
describe('App', () => {
  let page: AppPage;

  beforeEach(() => {
    page = new AppPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getMainHeading()).toEqual('Hello, world!');
  });
});
// ReSharper restore TsNotResolved
