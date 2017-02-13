/*
 *  Copyright (c) 2014-Present, Facebook, Inc.
 *  All rights reserved.
 *
 *  This source code is licensed under the BSD-style license found in the
 *  LICENSE file in the root directory of this source tree. An additional grant 
 *  of patent rights can be found in the PATENTS file in the same directory.
 */

// Basic console shim. Caches all calls to console methods.
function MockConsole() {
    this._calls = [];
    ['log', 'error', 'warn', 'debug', 'info', 'dir', 'group', 'groupEnd', 'groupCollapsed'].forEach(function (methodName) {
        this[methodName] = this._handleCall.bind(this, methodName);
    }, this);
}
MockConsole.prototype = {
    _handleCall: function (methodName/*, ...args*/) {
        var serializedArgs = [];
        for (var i = 1; i < arguments.length; i++) {
            serializedArgs.push(JSON.stringify(arguments[i]));
        }
        this._calls.push({
            method: methodName,
            args: serializedArgs
        });
    },
    _formatCall: function (call) {
        return 'console.' + call.method + '("[server]", ' + call.args.join(', ') + ');';
    },
    getCalls: function () {
        var output = this._calls.map(this._formatCall).join('\n');
        this._calls = [];
        return output;
    }
};

var console = new MockConsole();

var window = this || {};