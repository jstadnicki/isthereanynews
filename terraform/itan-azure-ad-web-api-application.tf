resource "azuread_application" "itan-ad-application-api" {
  name = "itan-api"
  owners = [var.subscription-owner-id]
  type = "webapp/api"
  identifier_uris = ["https://isthereanynewscodeblast.onmicrosoft.com/api"]
  oauth2_allow_implicit_flow = false

  oauth2_permissions {
    admin_consent_description = "application_writer."
    admin_consent_display_name = "application_writer"
    is_enabled = true
    type = "Admin"
    user_consent_description = "application_writer."
    user_consent_display_name = "application_writer"
    value = var.scope-reader
  }

  oauth2_permissions {
    admin_consent_description = "application_reader."
    admin_consent_display_name = "application_reader"
    is_enabled = true
    type = "Admin"
    user_consent_description = "application_reader."
    user_consent_display_name = "application_reader"
    value = var.scope-writer
  }
}