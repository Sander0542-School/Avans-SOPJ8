/* eslint-disable @typescript-eslint/no-unused-vars */
declare namespace Cypress {
  interface Chainable {
    login(userType: string): void
    logout(): void
    consent(): void
  }
}
