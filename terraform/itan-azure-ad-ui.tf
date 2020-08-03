resource "azuread_application" "itan-web-ui" {
  name = "itan-ui"
  owners = ["65748239-c3de-4eaf-9379-ef295093119e"]
  type = "webapp/api"
  identifier_uris = ["https://isthereanynewscodeblast.onmicrosoft.com/ui"]
  reply_urls = ["http://localhost:4200"]
  oauth2_allow_implicit_flow = false

  required_resource_access {
    resource_app_id = azuread_application.itan-web-api-application.application_id

    dynamic "resource_access" {
      iterator = item
      for_each = azuread_application.itan-web-api-application.oauth2_permissions

      content {
        id = item.value["id"]
        type = "Scope"
      }
    }
  }
}

