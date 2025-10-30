import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { GenericCrudService } from "src/app/core/services/generic-crud.service";
import { Category } from "src/app/features/category/models/category";

@Injectable()
export class CategoryService extends GenericCrudService<Category> {

     constructor(protected override http: HttpClient) {
      super(http, 'Category', 'category');
  }
}