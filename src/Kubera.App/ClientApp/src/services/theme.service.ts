import { Injectable, Renderer2, RendererFactory2 } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  public dark = 'dark';
  public light = 'light';

  private renderer: Renderer2;
  private colorScheme: string;
  // Define prefix for more clear and readable styling classes in scss files
  private colorSchemePrefix = 'color-scheme-';

  constructor(rendererFactory: RendererFactory2) {
    this.renderer = rendererFactory.createRenderer(null, null);
  }

  public load() {
      this.getColorScheme();
      this.renderer.addClass(document.body, this.colorSchemePrefix + this.colorScheme);
  }

  public update(scheme: string) {
      this.setColorScheme(scheme);
      // Remove the old color-scheme class
      this.renderer.removeClass( document.body, this.colorSchemePrefix + (this.colorScheme === this.dark ? this.light: this.dark) );
      // Add the new / current color-scheme class
      this.renderer.addClass(document.body, this.colorSchemePrefix + scheme);
  }

  public currentActive() {
      return this.colorScheme;
  }


  private detectPrefersColorScheme() {
    // Detect if prefers-color-scheme is supported
     if (window.matchMedia('(prefers-color-scheme)').media !== 'not all') {
         // Set colorScheme to Dark if prefers-color-scheme is dark. Otherwise set to light.
         this.colorScheme = window.matchMedia('(prefers-color-scheme: dark)').matches ? this.dark : this.light;
    } else {
         // If browser dont support prefers-color-scheme, set it as default to dark
        this.colorScheme = this.dark;
    }
  }

  private setColorScheme(scheme) {
      this.colorScheme = scheme;
      // Save prefers-color-scheme to localStorage
      localStorage.setItem('prefers-color', scheme);
  }

  private getColorScheme() {
      // Check if any prefers-color-scheme is stored in localStorage
      if (localStorage.getItem('prefers-color')) {
          // Save prefers-color-scheme from localStorage
          this.colorScheme = localStorage.getItem('prefers-color');
      } else {
          // If no prefers-color-scheme is stored in localStorage, Try to detect OS default prefers-color-scheme
          this.detectPrefersColorScheme();
      }
  }
}
