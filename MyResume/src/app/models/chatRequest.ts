import { RequestModel } from '../models/request';

export interface ChatRequestModel extends RequestModel {
  userRequestPromt: string;
  aiResponseMessage: string;
}
