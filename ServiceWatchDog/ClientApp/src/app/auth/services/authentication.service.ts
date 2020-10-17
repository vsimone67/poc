import { Injectable } from "@angular/core";
import { HttpService } from "@shared/service/http-service.service";
import { AppSettingsService } from "@shared/service/app-settings.service";
import { User } from '@auth/models/user';
import { SiteConstants } from '@constants/siteConstants';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({ providedIn: "root" })
export class AuthenticationService {
  private _apiUrl: string;
  private _authUrl: string;

  constructor(
    private _httpService: HttpService,
    private _appSettingsService: AppSettingsService,
  ) { }

  async login() {
    this._apiUrl = this._appSettingsService.GetValue(SiteConstants.authorizationApiUrl);
    this._authUrl = `${this._apiUrl}/${SiteConstants.authorizationApiController}`;

    localStorage.removeItem(SiteConstants.UserToken);
    var user = await this._httpService.getData<User>(
      `${this._authUrl}/login`
    );
    if (user && user.token) {
      localStorage.setItem(SiteConstants.UserToken, JSON.stringify(user));
    }
  }

  logout() {
    // remove user from local storage to log user out
    localStorage.removeItem(SiteConstants.UserToken);
  }

  isUserInRole(roleToCheck: string) {
    var isUserInRole: boolean = false;
    var currentUser: User = JSON.parse(
      localStorage.getItem(SiteConstants.UserToken)
    );

    if (currentUser) {
      //go through all the roles a user belongs to and see if they are allowed to access this route
      for (var i = 0; i < currentUser.roles.length; i++) {
        var role = currentUser.roles[i].role;

        if (role === roleToCheck) {
          isUserInRole = true;
          break;
        }
      }
    }
    return isUserInRole;
  }

  isUserInRoles(rolesToCheck: Array<string>) {
    var isUserInRole: boolean = false;

    //go through all the roles a user belongs to and see if they are allowed to access this route
    for (var i = 0; i < rolesToCheck.length; i++) {


      if (this.isUserInRole(rolesToCheck[i])) {
        isUserInRole = true;
        break;
      }
    }

    return isUserInRole;
  }

  isTokenExpired() {
    var tokenIsExpired: boolean = true;
    var currentUser: User = JSON.parse(
      localStorage.getItem(SiteConstants.UserToken)
    );

    if (currentUser && currentUser.token) {
      var jwtHelper = new JwtHelperService();
      tokenIsExpired = jwtHelper.isTokenExpired(currentUser.token);
    }
    return tokenIsExpired;
  }
}
