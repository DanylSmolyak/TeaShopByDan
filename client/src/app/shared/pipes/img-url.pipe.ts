import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'imgUrl'
})
export class ImgUrlPipe implements PipeTransform {

  transform(value: string | null | undefined): string | null | undefined {
    if (!value) return null;
    return `http://localhost:5097/${value}`;
  }

}
