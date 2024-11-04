import { VisibilityModel } from "../enums/visibility.model";

export class UploadStoryModel {
    public userId: string;
    public visibility: VisibilityModel;
    public file: File;
}