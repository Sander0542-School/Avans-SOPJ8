describe('Homepage', () => {
  beforeEach(() => {
    cy.visit('/');
    cy.get('.accept-policy > span').click();
  });

  it('Has sidebar', () => {
    cy.get('.sidebar').should('exist');
  });

  it('Has language picker', () => {
    cy.get('nav  div.sidebar-footer.text-center i.fa-language').click();
    cy.get('#cultureModal').should('be.visible');
  });

  it('Change language to Dutch', () => {
    cy.get('nav  div.sidebar-footer.text-center i.fa-language').click();
    cy.get('#cultureModal').should('be.visible')
      .contains('Nederlands').click();

    cy.get('.sidebar-footer').should('contain', 'Verander taal');
  });

  it('Change language to English', () => {
    cy.get('nav  div.sidebar-footer.text-center i.fa-language').click();
    cy.get('#cultureModal').should('be.visible')
      .contains('English').click();

    cy.get('.sidebar-footer').should('contain', 'Change Language');
  });
});
