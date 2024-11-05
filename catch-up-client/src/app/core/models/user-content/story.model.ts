import { MediaTypeModel } from "../enums/media-type.model";
import { VisibilityModel } from "../enums/visibility.model";

export class StoryModel {
    public id: number;
    public createdAt: Date;
    public expiresAt: Date;
    public visibility: VisibilityModel;
    public userId: string;
    // public mediaUrl: string;
    public mediaContent: { type: MediaTypeModel; mediaUrl: string };
    public viewCount: number;
}