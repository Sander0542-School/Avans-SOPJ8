describe('Set availability', () => {
  it('Set hours for Stijn', () => {
    cy.login('acceptance-tests/logins/stefan');

    cy.get('.sidebar a[href="/AdditionalWork"]').should('be.visible').click();

    cy.get('#Work_StartTime').eq(0).should('be.visible').type('10:00');
    cy.get('#Work_EndTime').eq(0).should('be.visible').type('15:00');
  });
});
