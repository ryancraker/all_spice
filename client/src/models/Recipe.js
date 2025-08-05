export class Recipe {
	constructor(data) {
		this.id = data.id;
		this.createdAt = new Date(data.createdAt);
		this.updatedAt = new Date(data.updatedAt);
		this.creatorId = data.creatorId;
		this.creator = data.creator;
		this.category = data.category;
		this.favoriteCount = data.favoriteCount;
		this.imgUrl = data.img;
		this.instructions = data.instructions;
		this.title = data.title;
		this.favoriteId = data.favoriteId ? data.favoriteId : null;
	}
}
