/* global browser describe it */
var assert = require('assert')

describe('Party web', () => {
    it('should allow adding a person and updating', () => {
        return browser.url('/')
            .getTitle().then(function (title) {
                assert.equal(title, 'Home Page - WebApplicationParty')
            })
            .click('[href="/Person"]')

            // Add Person
            .waitForExist('[type="submit"]')
            .setValue('[name="FirstName"]', 'Jonny')
            .setValue('[name="Surname"]', 'Test')
            .setValue('[name="DateOfBirth"]', '01/01/1980')
            .setValue('[name="EmailAddress"]', 'test@test.com')
            .click('[type="submit"]')

            .waitForExist('=Jonny Test')
            .click('=Jonny Test')

            // View/Update Person
            .setValue('[name="Surname"]', 'Test1')
            .click('[type="submit"]')

            .waitForExist('=Jonny Test')
    })
})