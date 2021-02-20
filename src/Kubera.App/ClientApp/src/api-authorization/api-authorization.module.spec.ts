import { ApiAuthorizationModule } from './api-authorization.module';

// ReSharper disable TsNotResolved
describe('ApiAuthorizationModule', () => {
  let apiAuthorizationModule: ApiAuthorizationModule;

  beforeEach(() => {
    apiAuthorizationModule = new ApiAuthorizationModule();
  });

  it('should create an instance', () => {
    expect(apiAuthorizationModule).toBeTruthy();
  });
});
// ReSharper restore TsNotResolved
