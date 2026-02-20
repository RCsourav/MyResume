import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AppConfigService {
  globalLoginId = 0;
  globalUsername = '';
  globalEmail = '';
  globalSessionTime = 1800;
}

export const environment = {
  aiFunctionUrl: 'https://my-resume-sourav-rc-ai.azurewebsites.net/api/ai/AgentChat',
  loginFunctionUrl: 'https://my-resume-sourav-rc-ai.azurewebsites.net/api/api/LogOn',
  logoffFunctionUrl: 'https://my-resume-sourav-rc-ai.azurewebsites.net/api/api/LogOff',
  getActiveSessionFunctionUrl: 'https://my-resume-sourav-rc-ai.azurewebsites.net/api/api/GetActiveSession',
  saveChatDataFunctionUrl: 'https://my-resume-sourav-rc-ai.azurewebsites.net/api/api/SaveChatData',
  updatedActivityFunctionUrl: 'https://my-resume-sourav-rc-ai.azurewebsites.net/api/api/UpdatedActivity'
};
