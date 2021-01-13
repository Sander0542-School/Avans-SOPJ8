describe('Approve Schedule', () => {
  beforeEach(() => {
    // Keeps session alive through this "describe" method
    Cypress.Cookies.preserveOnce('.AspNetCore.Identity.Application');
  });

  it('Login for manager user', () => {
    cy.login('manager');
  });

  it('Navigate to branch schedule', () => {
    cy.visit('Branches/1/Schedule');

    cy.get('#departmentGroup').should('exist').click();
    cy.get('div[aria-labelledby="departmentGroup"] > a[href*="VAK"]').should('exist').click();
  });

  it('Approve the schedule', () => {
    cy.get('button[data-target="#approveScheduleModal"]').should('exist').click();

    cy.get('#approveScheduleModal').should('be.visible');

    cy.get('#approveScheduleModal button[type="submit"]').should('be.visible').click();

    cy.get('.alert.alert-success').should('exist');
  });
});
