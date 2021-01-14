describe('Set the norms for branch 1 for forecast testing', () => {
  beforeEach(() => {
    // Keeps session alive through this "describe" method
    Cypress.Cookies.preserveOnce('.AspNetCore.Identity.Application');
  });

  it('Login as manager', () => {
    cy.login('manager');
  });

  it('Go to create forecast page', () => {
    cy.visit('Branches/1/Forecast/ChangeNorms');

    cy.fixture('norms').then((norms) => {
      cy.get(':nth-child(1) > #ForecastStandardValue').clear().type(norms.unload_coli);
      cy.get(':nth-child(2) > #ForecastStandardValue').clear().type(norms.stocking_shelves);
      cy.get(':nth-child(3) > #ForecastStandardValue').clear().type(norms.cash_register);
      cy.get(':nth-child(4) > #ForecastStandardValue').clear().type(norms.fresh);
      cy.get(':nth-child(5) > #ForecastStandardValue').clear().type(norms.face_shelves);
    });
    cy.get('input[type=submit]').click();

    cy.location('pathname').should('contain', '/Forecast/Index');
  });
});
