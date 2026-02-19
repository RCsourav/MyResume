import { RequestModel } from '../models/request';

export interface ResponseModel extends RequestModel {
  message: string;
  isSuccessful: string;
  loggedOnTime: string;
  lastActivityTime: string;
  isActive: boolean;
  returnCode: number;
  ipAddress: string;
}
