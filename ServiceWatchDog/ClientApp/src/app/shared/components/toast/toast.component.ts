import { Component, OnInit, AfterViewInit, Input } from '@angular/core';
import { MessageService, Message } from 'primeng/api';
@Component({
    selector: 'toast',
    templateUrl: './toast.component.html',
    styleUrls: ['./toast.component.css']
})
export class ToastComponent implements OnInit, AfterViewInit {
    @Input() keepAlive: number = 7000;
    timeout: any;
    
    constructor(private _messageService: MessageService) {
    }

    ngOnInit() {
        
    }

    ngAfterViewInit() {
        this.initTimeout();
        
    }

    initTimeout() {
        this.timeout = setTimeout(() => {
            this._messageService.clear();
        }, this.keepAlive);
    }

    clearTimeout() {
        if (this.timeout) {
            clearTimeout(this.timeout);
            this.timeout = null;
        }
    }
}
