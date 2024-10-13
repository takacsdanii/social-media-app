import { VisibilityModel } from "../enums/visibility.model";

export class UploadPostModel {
    public userId: string;
    public description: string | null;
    public visibility: VisibilityModel;
}