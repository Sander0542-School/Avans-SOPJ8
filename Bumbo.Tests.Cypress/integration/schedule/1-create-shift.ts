describe('Create Shift', () => {
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
    cy.get('div[aria-labelledby="departmentGroup"] > a[href*="VAK"]').should('be.visible').click();

    cy.get('#dateNextWeek').should('be.visible').click();
  });

  it('Create the new Shift', () => {
    cy.get('table tbody tr:first [onclick*="shiftModal"]:first').should('exist').click();
    cy.get('#shiftEditModal').should('be.visible');

    cy.get('#InputShift_StartTime').type('16:00');
    cy.wait(100);
    cy.get('#InputShift_EndTime').type('21:00');

    cy.get('#shiftEditModal button[type="submit"]').should('be.visible').click();

    cy.get('.alert.alert-success').should('exist');
  });
});
