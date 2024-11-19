import {Component, OnDestroy, OnInit} from '@angular/core';
import {interval, Subscription} from "rxjs";
export interface NewsItem {
  id: number;
  title: string;
  description: string;
  imageUrl: string;
}
@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.css']
})
export class NewsComponent implements OnInit, OnDestroy {
  newsItems: NewsItem[] = [
    {
      id: 1,
      title: 'Тільки в нас',
      description: '"Добро пожаловать в мир чая! Наслаждайтесь ароматом каждого глотка."',
      imageUrl: 'assets/Images/news1.jpg'
    },
    {
      id: 2,
      title: 'Тільки в нас',
      description: '"Эксклюзивные вкусы для ценителей!"',
      imageUrl: 'assets/Images/news2.jpg'
    },
    {
      id: 3,
      title: 'Тільки в нас',
      description: "Доставляем чай и кофе по всему миру!",
      imageUrl: 'assets/Images/news3.jpg'
    }
  ];

  currentIndex = 0;
  private autoSlideSubscription?: Subscription;
  autoSlideInterval = 5000; // 5 seconds

  ngOnInit() {
    this.startAutoSlide();
  }

  ngOnDestroy() {
    if (this.autoSlideSubscription) {
      this.autoSlideSubscription.unsubscribe();
    }
  }

  startAutoSlide() {
    this.autoSlideSubscription = interval(this.autoSlideInterval).subscribe(() => {
      this.nextSlide();
    });
  }

  stopAutoSlide() {
    if (this.autoSlideSubscription) {
      this.autoSlideSubscription.unsubscribe();
    }
  }

  previousSlide() {
    this.stopAutoSlide();
    this.currentIndex = this.currentIndex > 0 ? this.currentIndex - 1 : this.newsItems.length - 1;
    this.startAutoSlide();
  }

  nextSlide() {
    this.currentIndex = this.currentIndex < this.newsItems.length - 1 ? this.currentIndex + 1 : 0;
  }

  goToSlide(index: number) {
    this.stopAutoSlide();
    this.currentIndex = index;
    this.startAutoSlide();
  }
}
