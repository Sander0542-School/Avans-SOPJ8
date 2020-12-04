describe('Homepage', () => {
  it('Has sidebar', () => {
    cy.visit('/');
    cy.get('.sidebar').should('exist');
  });
});
