resource "azuread_application" "itan-web-ui-ad-application" {
  name = "itan-ui"
  owners = [var.subscription-owner-id]
  type = "webapp/api"
  identifier_uris = ["https://isthereanynewscodeblast.onmicrosoft.com/ui"]
  reply_urls = ["http://localhost:4200"]
  oauth2_allow_implicit_flow = false

  required_resource_access {
    resource_app_id = azuread_application.itan-web-api-ad-application.application_id

    dynamic "resource_access" {
      iterator = item
      for_each = azuread_application.itan-web-api-ad-application.oauth2_permissions

      content {
        id = item.value["id"]
        type = "Scope"
      }
    }
  }
}

