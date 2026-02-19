import { ResponseModel } from '../models/response';

export interface ChatResponseModel extends ResponseModel {
  userRequestPromt: string;
  aiResponseMessage: string;
}
