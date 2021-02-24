"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Endpoints = exports.EndpointsDelete = exports.EndpointsGet = void 0;
var EndpointsGet = /** @class */ (function () {
    function EndpointsGet(base) {
        this.base = base;
        this.transactions = this.base + "/transaction";
    }
    return EndpointsGet;
}());
exports.EndpointsGet = EndpointsGet;
var EndpointsDelete = /** @class */ (function () {
    function EndpointsDelete(base) {
        this.base = base;
        this.transaction = this.base + "/transaction";
    }
    return EndpointsDelete;
}());
exports.EndpointsDelete = EndpointsDelete;
var Endpoints = /** @class */ (function () {
    function Endpoints() {
        this.base = '/api/v1';
        this.get = new EndpointsGet(this.base);
        this.delete = new EndpointsDelete(this.base);
    }
    return Endpoints;
}());
exports.Endpoints = Endpoints;
//# sourceMappingURL=endpoints.model.js.map