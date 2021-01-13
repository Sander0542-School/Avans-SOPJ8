describe('Approve Schedule', () => {
  beforeEach(() => {
    // Keeps session alive through this "describe" method
    Cypress.Cookies.preserveOnce('.AspNetCore.Identity.Application');
  });

  it('Login for manager user', () => {
    cy.login('employee');
  });

  it('Navigate to personal schedule', () => {
    cy.visit('Branches/1/Schedule/Personal');
  });

  it('Look for shift in schedule', () => {
    cy.get('#calendar').should('be.visible');

    cy.get('#calendar .fc-event-container').should('be.visible').click();
  });

  it('Offer shift', () => {
    cy.get('#offerShiftModal').should('be.visible');

    cy.get('#offerShiftModal button[type="submit"]').should('be.visible').click();

    cy.get('.alert.alert-success').should('exist');
  });
});
