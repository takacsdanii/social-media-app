import { MediaTypeModel } from "../enums/media-type.model";
import { VisibilityModel } from "../enums/visibility.model";

export class PostModel {
    public id: number;
    public description: string | null;
    public createdAt: Date;
    public visibility: VisibilityModel;
    public userId: string;
    // public mediaUrls: string[];
    public mediaContents: { type: MediaTypeModel; mediaUrl: string }[];
    public likeCount: number;
    public commentCount: number;
}