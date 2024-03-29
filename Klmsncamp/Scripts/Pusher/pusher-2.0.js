﻿/*!
 * Pusher JavaScript Library v2.0.2
 * http://pusherapp.com/
 *
 * Copyright 2013, Pusher
 * Released under the MIT licence.
 */

(function () {
    function b(a, h) {
        var e = this; this.options = h || {}; this.key = a; this.channels = new b.Channels; this.global_emitter = new b.EventsDispatcher; this.sessionID = Math.floor(Math.random() * 1E9); c(this.key); this.connection = new b.ConnectionManager(this.key, b.Util.extend({
            getStrategy: function (a) { return b.StrategyBuilder.build(b.getDefaultStrategy(), b.Util.extend({}, e.options, a)) }, getTimeline: function () {
                return new b.Timeline(e.key, e.sessionID, {
                    features: b.Util.getClientFeatures(), params: e.options.timelineParams ||
                    {}, limit: 25, level: b.Timeline.INFO, version: b.VERSION
                })
            }, getTimelineSender: function (a, d) { return e.options.disableStats ? null : new b.TimelineSender(a, { encrypted: e.isEncrypted() || !!d.encrypted, host: b.stats_host, path: "/timeline" }) }, activityTimeout: b.activity_timeout, pongTimeout: b.pong_timeout, unavailableTimeout: b.unavailable_timeout
        }, this.options, { encrypted: this.isEncrypted() })); this.connection.bind("connected", function () { e.subscribeAll() }); this.connection.bind("message", function (a) {
            var d = a.event.indexOf("pusher_internal:") ===
            0; if (a.channel) { var b = e.channel(a.channel); b && b.emit(a.event, a.data) } d || e.global_emitter.emit(a.event, a.data)
        }); this.connection.bind("disconnected", function () { e.channels.disconnect() }); this.connection.bind("error", function (a) { b.warn("Error", a) }); b.instances.push(this); b.isReady && e.connect()
    } function c(a) { (a === null || a === void 0) && b.warn("Warning", "You must pass your app key when you instantiate Pusher.") } var a = b.prototype; b.instances = []; b.isReady = !1; b.debug = function () {
        b.log && b.log(b.Util.stringify.apply(this,
        arguments))
    }; b.warn = function () { window.console && window.console.warn ? window.console.warn(b.Util.stringify.apply(this, arguments)) : b.log && b.log(b.Util.stringify.apply(this, arguments)) }; b.ready = function () { b.isReady = !0; for (var a = 0, c = b.instances.length; a < c; a++) b.instances[a].connect() }; a.channel = function (a) { return this.channels.find(a) }; a.connect = function () { this.connection.connect() }; a.disconnect = function () { this.connection.disconnect() }; a.bind = function (a, b) { this.global_emitter.bind(a, b); return this }; a.bind_all =
    function (a) { this.global_emitter.bind_all(a); return this }; a.subscribeAll = function () { for (var a in this.channels.channels) this.channels.channels.hasOwnProperty(a) && this.subscribe(a) }; a.subscribe = function (a) { var b = this, c = this.channels.add(a, this); this.connection.state === "connected" && c.authorize(this.connection.socket_id, this.options, function (f, g) { f ? c.emit("pusher:subscription_error", g) : b.send_event("pusher:subscribe", { channel: a, auth: g.auth, channel_data: g.channel_data }) }); return c }; a.unsubscribe = function (a) {
        this.channels.remove(a);
        this.connection.state === "connected" && this.send_event("pusher:unsubscribe", { channel: a })
    }; a.send_event = function (a, b, c) { return this.connection.send_event(a, b, c) }; a.isEncrypted = function () { return b.Util.getDocumentLocation().protocol === "https:" ? !0 : !!this.options.encrypted }; this.Pusher = b
}).call(this);
(function () {
    Pusher.Util = {
        now: function () { return Date.now ? Date.now() : (new Date).valueOf() }, extend: function (b) { for (var c = 1; c < arguments.length; c++) { var a = arguments[c], d; for (d in a) b[d] = a[d] && a[d].constructor && a[d].constructor === Object ? Pusher.Util.extend(b[d] || {}, a[d]) : a[d] } return b }, stringify: function () { for (var b = ["Pusher"], c = 0; c < arguments.length; c++) typeof arguments[c] === "string" ? b.push(arguments[c]) : window.JSON === void 0 ? b.push(arguments[c].toString()) : b.push(JSON.stringify(arguments[c])); return b.join(" : ") },
        arrayIndexOf: function (b, c) { var a = Array.prototype.indexOf; if (b === null) return -1; if (a && b.indexOf === a) return b.indexOf(c); for (var a = 0, d = b.length; a < d; a++) if (b[a] === c) return a; return -1 }, keys: function (b) { var c = [], a; for (a in b) Object.prototype.hasOwnProperty.call(b, a) && c.push(a); return c }, apply: function (b, c) { for (var a = 0; a < b.length; a++) c(b[a], a, b) }, objectApply: function (b, c) { for (var a in b) Object.prototype.hasOwnProperty.call(b, a) && c(b[a], a, b) }, map: function (b, c) {
            for (var a = [], d = 0; d < b.length; d++) a.push(c(b[d],
            d, b, a)); return a
        }, mapObject: function (b, c) { var a = {}, d; for (d in b) Object.prototype.hasOwnProperty.call(b, d) && (a[d] = c(b[d])); return a }, filter: function (b, c) { for (var c = c || function (a) { return !!a }, a = [], d = 0; d < b.length; d++) c(b[d], d, b, a) && a.push(b[d]); return a }, filterObject: function (b, c) { var c = c || function (a) { return !!a }, a = {}, d; for (d in b) Object.prototype.hasOwnProperty.call(b, d) && c(b[d], d, b, a) && (a[d] = b[d]); return a }, flatten: function (b) {
            var c = [], a; for (a in b) Object.prototype.hasOwnProperty.call(b, a) && c.push([a,
            b[a]]); return c
        }, any: function (b, c) { for (var a = 0; a < b.length; a++) if (c(b[a], a, b)) return !0; return !1 }, all: function (b, c) { for (var a = 0; a < b.length; a++) if (!c(b[a], a, b)) return !1; return !0 }, method: function (b) { var c = Array.prototype.slice.call(arguments, 1); return function (a) { return a[b].apply(a, c.concat(arguments)) } }, getDocument: function () { return document }, getDocumentLocation: function () { return Pusher.Util.getDocument().location }, getLocalStorage: function () { return window.localStorage }, getClientFeatures: function () {
            return Pusher.Util.keys(Pusher.Util.filterObject({
                ws: Pusher.WSTransport,
                flash: Pusher.FlashTransport
            }, function (b) { return b.isSupported() }))
        }
    }
}).call(this);
(function () {
    Pusher.VERSION = "2.0.2"; Pusher.PROTOCOL = 6; Pusher.host = "ws.pusherapp.com"; Pusher.ws_port = 80; Pusher.wss_port = 443; Pusher.sockjs_host = "sockjs.pusher.com"; Pusher.sockjs_http_port = 80; Pusher.sockjs_https_port = 443; Pusher.sockjs_path = "/pusher"; Pusher.stats_host = "stats.pusher.com"; Pusher.channel_auth_endpoint = "/pusher/auth"; Pusher.cdn_http = "http://js.pusher.com/"; Pusher.cdn_https = "https://d3dy5gmtp8yhk7.cloudfront.net/"; Pusher.dependency_suffix = ".min"; Pusher.channel_auth_transport = "ajax"; Pusher.activity_timeout =
    12E4; Pusher.pong_timeout = 3E4; Pusher.unavailable_timeout = 1E4; Pusher.getDefaultStrategy = function () {
        return [[":def", "ws_options", { hostUnencrypted: Pusher.host + ":" + Pusher.ws_port, hostEncrypted: Pusher.host + ":" + Pusher.wss_port }], [":def", "sockjs_options", { hostUnencrypted: Pusher.sockjs_host + ":" + Pusher.sockjs_http_port, hostEncrypted: Pusher.sockjs_host + ":" + Pusher.sockjs_https_port }], [":def", "timeouts", { loop: !0, timeout: 15E3, timeoutLimit: 6E4 }], [":def", "ws_manager", [":transport_manager", { lives: 2 }]], [":def_transport",
        "ws", "ws", 3, ":ws_options", ":ws_manager"], [":def_transport", "flash", "flash", 2, ":ws_options", ":ws_manager"], [":def_transport", "sockjs", "sockjs", 1, ":sockjs_options"], [":def", "ws_loop", [":sequential", ":timeouts", ":ws"]], [":def", "flash_loop", [":sequential", ":timeouts", ":flash"]], [":def", "sockjs_loop", [":sequential", ":timeouts", ":sockjs"]], [":def", "strategy", [":cached", 18E5, [":first_connected", [":if", [":is_supported", ":ws"], [":best_connected_ever", ":ws_loop", [":delayed", 2E3, [":sockjs_loop"]]], [":if", [":is_supported",
        ":flash"], [":best_connected_ever", ":flash_loop", [":delayed", 2E3, [":sockjs_loop"]]], [":sockjs_loop"]]]]]]]
    }
}).call(this); (function () { function b(b) { var a = function (a) { Error.call(this, a); this.name = b }; Pusher.Util.extend(a.prototype, Error.prototype); return a } Pusher.Errors = { UnsupportedTransport: b("UnsupportedTransport"), UnsupportedStrategy: b("UnsupportedStrategy"), TransportPriorityTooLow: b("TransportPriorityTooLow"), TransportClosed: b("TransportClosed") } }).call(this);
(function () {
    function b(a) { this.callbacks = new c; this.global_callbacks = []; this.failThrough = a } function c() { this._callbacks = {} } var a = b.prototype; a.bind = function (a, b) { this.callbacks.add(a, b); return this }; a.bind_all = function (a) { this.global_callbacks.push(a); return this }; a.unbind = function (a, b) { this.callbacks.remove(a, b); return this }; a.emit = function (a, b) {
        var c; for (c = 0; c < this.global_callbacks.length; c++) this.global_callbacks[c](a, b); var f = this.callbacks.get(a); if (f && f.length > 0) for (c = 0; c < f.length; c++) f[c](b);
        else this.failThrough && this.failThrough(a, b); return this
    }; c.prototype.get = function (a) { return this._callbacks[this._prefix(a)] }; c.prototype.add = function (a, b) { var c = this._prefix(a); this._callbacks[c] = this._callbacks[c] || []; this._callbacks[c].push(b) }; c.prototype.remove = function (a, b) { if (this.get(a)) { var c = Pusher.Util.arrayIndexOf(this.get(a), b), f = this._callbacks[this._prefix(a)].slice(0); f.splice(c, 1); this._callbacks[this._prefix(a)] = f } }; c.prototype._prefix = function (a) { return "_" + a }; Pusher.EventsDispatcher =
    b
}).call(this);
(function () {
    function b(a) { this.options = a; this.loading = {}; this.loaded = {} } function c(a, b) { Pusher.Util.getDocument().addEventListener ? a.addEventListener("load", b, !1) : a.attachEvent("onreadystatechange", function () { (a.readyState === "loaded" || a.readyState === "complete") && b() }) } function a(a, b) {
        var d = Pusher.Util.getDocument(), g = d.getElementsByTagName("head")[0], d = d.createElement("script"); d.setAttribute("src", a); d.setAttribute("type", "text/javascript"); d.setAttribute("async", !0); c(d, function () { setTimeout(b, 0) });
        g.appendChild(d)
    } var d = b.prototype; d.load = function (b, d) { var c = this; this.loaded[b] ? d() : (this.loading[b] || (this.loading[b] = []), this.loading[b].push(d), this.loading[b].length > 1 || a(this.getPath(b), function () { for (var a = 0; a < c.loading[b].length; a++) c.loading[b][a](); delete c.loading[b]; c.loaded[b] = !0 })) }; d.getRoot = function (a) { var b = Pusher.Util.getDocumentLocation().protocol; return (a && a.encrypted || b === "https:" ? this.options.cdn_https : this.options.cdn_http).replace(/\/*$/, "") + "/" + this.options.version }; d.getPath =
    function (a, b) { return this.getRoot(b) + "/" + a + this.options.suffix + ".js" }; Pusher.DependencyLoader = b
}).call(this);
(function () { function b() { Pusher.ready() } function c(a) { document.body ? a() : setTimeout(function () { c(a) }, 0) } function a() { c(b) } Pusher.Dependencies = new Pusher.DependencyLoader({ cdn_http: Pusher.cdn_http, cdn_https: Pusher.cdn_https, version: Pusher.VERSION, suffix: Pusher.dependency_suffix }); if (!window.WebSocket && window.MozWebSocket) window.WebSocket = window.MozWebSocket; window.JSON ? a() : Pusher.Dependencies.load("json2", a) })();
(function () { function b(a, b) { var c = this; this.timeout = setTimeout(function () { if (c.timeout !== null) b(), c.timeout = null }, a) } var c = b.prototype; c.isRunning = function () { return this.timeout !== null }; c.ensureAborted = function () { if (this.timeout) clearTimeout(this.timeout), this.timeout = null }; Pusher.Timer = b }).call(this);
(function () {
    for (var b = String.fromCharCode, c = 0; c < 64; c++) "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/".charAt(c); var a = function (a) { var d = a.charCodeAt(0); return d < 128 ? a : d < 2048 ? b(192 | d >>> 6) + b(128 | d & 63) : b(224 | d >>> 12 & 15) + b(128 | d >>> 6 & 63) + b(128 | d & 63) }, d = function (a) {
        var b = [0, 2, 1][a.length % 3], a = a.charCodeAt(0) << 16 | (a.length > 1 ? a.charCodeAt(1) : 0) << 8 | (a.length > 2 ? a.charCodeAt(2) : 0); return ["ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/".charAt(a >>> 18), "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/".charAt(a >>>
        12 & 63), b >= 2 ? "=" : "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/".charAt(a >>> 6 & 63), b >= 1 ? "=" : "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/".charAt(a & 63)].join("")
    }, h = window.btoa || function (a) { return a.replace(/[\s\S]{1,3}/g, d) }; Pusher.Base64 = { encode: function (b) { return h(b.replace(/[^\x00-\x7F]/g, a)) } }
}).call(this);
(function () {
    function b(a) { this.options = a } function c(a) { return Pusher.Util.mapObject(a, function (a) { typeof a === "object" && (a = JSON.stringify(a)); return encodeURIComponent(Pusher.Base64.encode(a.toString())) }) } b.send = function (a, b) { var c = new Pusher.JSONPRequest({ url: a.url, receiver: a.receiverName, tagPrefix: a.tagPrefix }), f = a.receiver.register(function (a, d) { c.cleanup(); b(a, d) }); return c.send(f, a.data, function (b) { var c = a.receiver.unregister(f); c && c(b) }) }; var a = b.prototype; a.send = function (a, b, e) {
        if (this.script) return !1;
        var f = this.options.tagPrefix || "_pusher_jsonp_", b = Pusher.Util.extend({}, b, { receiver: this.options.receiver }), b = Pusher.Util.map(Pusher.Util.flatten(c(Pusher.Util.filterObject(b, function (a) { return a !== void 0 }))), Pusher.Util.method("join", "=")).join("&"); this.script = document.createElement("script"); this.script.id = f + a; this.script.src = this.options.url + "/" + a + "?" + b; this.script.type = "text/javascript"; this.script.charset = "UTF-8"; this.script.onerror = this.script.onload = e; if (this.script.async === void 0 && document.attachEvent &&
        /opera/i.test(navigator.userAgent)) f = this.options.receiver || "Pusher.JSONP.receive", this.errorScript = document.createElement("script"), this.errorScript.text = f + "(" + a + ", true);", this.script.async = this.errorScript.async = !1; var g = this; this.script.onreadystatechange = function () { g.script && /loaded|complete/.test(g.script.readyState) && e(!0) }; a = document.getElementsByTagName("head")[0]; a.insertBefore(this.script, a.firstChild); this.errorScript && a.insertBefore(this.errorScript, this.script.nextSibling); return !0
    };
    a.cleanup = function () { if (this.script && this.script.parentNode) this.script.parentNode.removeChild(this.script), this.script = null; if (this.errorScript && this.errorScript.parentNode) this.errorScript.parentNode.removeChild(this.errorScript), this.errorScript = null }; Pusher.JSONPRequest = b
}).call(this);
(function () { function b() { this.lastId = 0; this.callbacks = {} } var c = b.prototype; c.register = function (a) { this.lastId++; var b = this.lastId; this.callbacks[b] = a; return b }; c.unregister = function (a) { if (this.callbacks[a]) { var b = this.callbacks[a]; delete this.callbacks[a]; return b } else return null }; c.receive = function (a, b, c) { (a = this.unregister(a)) && a(b, c) }; Pusher.JSONPReceiver = b; Pusher.JSONP = new b }).call(this);
(function () {
    function b(a, b, c) { this.key = a; this.session = b; this.events = []; this.options = c || {}; this.uniqueID = this.sent = 0 } var c = b.prototype; b.ERROR = 3; b.INFO = 6; b.DEBUG = 7; c.log = function (a, b) { if (this.options.level === void 0 || a <= this.options.level) this.events.push(Pusher.Util.extend({}, b, { timestamp: Pusher.Util.now(), level: a })), this.options.limit && this.events.length > this.options.limit && this.events.shift() }; c.error = function (a) { this.log(b.ERROR, a) }; c.info = function (a) { this.log(b.INFO, a) }; c.debug = function (a) {
        this.log(b.DEBUG,
        a)
    }; c.isEmpty = function () { return this.events.length === 0 }; c.send = function (a, b) { var c = this, e = {}; this.sent === 0 && (e = Pusher.Util.extend({ key: this.key, features: this.options.features, version: this.options.version }, this.options.params || {})); e.session = this.session; e.timeline = this.events; e = Pusher.Util.filterObject(e, function (a) { return a !== void 0 }); this.events = []; a(e, function (a, e) { a || c.sent++; b(a, e) }); return !0 }; c.generateUniqueID = function () { this.uniqueID++; return this.uniqueID }; Pusher.Timeline = b
}).call(this);
(function () { function b(a, b) { this.timeline = a; this.options = b || {} } var c = b.prototype; c.send = function (a) { if (!this.timeline.isEmpty()) { var b = this.options, c = "http" + (this.isEncrypted() ? "s" : "") + "://"; this.timeline.send(function (a, f) { return Pusher.JSONPRequest.send({ data: a, url: c + b.host + b.path, receiver: Pusher.JSONP }, f) }, a) } }; c.isEncrypted = function () { return !!this.options.encrypted }; Pusher.TimelineSender = b }).call(this);
(function () {
    function b(a) { this.strategies = a } function c(a, b, c) { var h = Pusher.Util.map(a, function (a, d, h, e) { return a.connect(b, c(d, e)) }); return { abort: function () { Pusher.Util.apply(h, d) }, forceMinPriority: function (a) { Pusher.Util.apply(h, function (b) { b.forceMinPriority(a) }) } } } function a(a) { return Pusher.Util.all(a, function (a) { return Boolean(a.error) }) } function d(a) { if (!a.error && !a.aborted) a.abort(), a.aborted = !0 } var h = b.prototype; h.isSupported = function () { return Pusher.Util.any(this.strategies, Pusher.Util.method("isSupported")) };
    h.connect = function (b, d) { return c(this.strategies, b, function (b, c) { return function (h, e) { (c[b].error = h) ? a(c) && d(!0) : (Pusher.Util.apply(c, function (a) { a.forceMinPriority(e.priority) }), d(null, e)) } }) }; Pusher.BestConnectedEverStrategy = b
}).call(this);
(function () {
    function b(a, b, c) { this.strategy = a; this.transports = b; this.ttl = c.ttl || 18E5; this.timeline = c.timeline } function c() { var a = Pusher.Util.getLocalStorage(); return a && a.pusherTransport ? JSON.parse(a.pusherTransport) : null } var a = b.prototype; a.isSupported = function () { return this.strategy.isSupported() }; a.connect = function (a, b) {
        var e = c(), f = [this.strategy]; if (e && e.timestamp + this.ttl >= Pusher.Util.now()) {
            var g = this.transports[e.transport]; g && (this.timeline.info({ cached: !0, transport: e.transport }), f.push(new Pusher.SequentialStrategy([g],
            { timeout: e.latency * 2, failFast: !0 })))
        } var i = Pusher.Util.now(), k = f.pop().connect(a, function j(c, e) { if (c) { var g = Pusher.Util.getLocalStorage(); g && delete g.pusherTransport; f.length > 0 ? (i = Pusher.Util.now(), k = f.pop().connect(a, j)) : b(c) } else { var g = Pusher.Util.now() - i, o = e.name, n = Pusher.Util.getLocalStorage(); if (n) n.pusherTransport = JSON.stringify({ timestamp: Pusher.Util.now(), transport: o, latency: g }); b(null, e) } }); return { abort: function () { k.abort() }, forceMinPriority: function (b) { a = b; k && k.forceMinPriority(b) } }
    }; Pusher.CachedStrategy =
    b
}).call(this); (function () { function b(a, b) { this.strategy = a; this.options = { delay: b.delay } } var c = b.prototype; c.isSupported = function () { return this.strategy.isSupported() }; c.connect = function (a, b) { var c = this.strategy, e, f = new Pusher.Timer(this.options.delay, function () { e = c.connect(a, b) }); return { abort: function () { f.ensureAborted(); e && e.abort() }, forceMinPriority: function (b) { a = b; e && e.forceMinPriority(b) } } }; Pusher.DelayedStrategy = b }).call(this);
(function () { function b(a) { this.strategy = a } var c = b.prototype; c.isSupported = function () { return this.strategy.isSupported() }; c.connect = function (a, b) { var c = this.strategy.connect(a, function (a, f) { f && c.abort(); b(a, f) }); return c }; Pusher.FirstConnectedStrategy = b }).call(this);
(function () { function b(a, b, c) { this.test = a; this.trueBranch = b; this.falseBranch = c } var c = b.prototype; c.isSupported = function () { return (this.test() ? this.trueBranch : this.falseBranch).isSupported() }; c.connect = function (a, b) { return (this.test() ? this.trueBranch : this.falseBranch).connect(a, b) }; Pusher.IfStrategy = b }).call(this);
(function () {
    function b(a, b) { this.strategies = a; this.loop = Boolean(b.loop); this.failFast = Boolean(b.failFast); this.timeout = b.timeout; this.timeoutLimit = b.timeoutLimit } var c = b.prototype; c.isSupported = function () { return Pusher.Util.any(this.strategies, Pusher.Util.method("isSupported")) }; c.connect = function (a, b) {
        var c = this, e = this.strategies, f = 0, g = this.timeout, i = null, k = function (l, j) {
            j ? b(null, j) : (f += 1, c.loop && (f %= e.length), f < e.length ? (g && (g *= 2, c.timeoutLimit && (g = Math.min(g, c.timeoutLimit))), i = c.tryStrategy(e[f],
            a, { timeout: g, failFast: c.failFast }, k)) : b(!0))
        }, i = this.tryStrategy(e[f], a, { timeout: g, failFast: this.failFast }, k); return { abort: function () { i.abort() }, forceMinPriority: function (b) { a = b; i && i.forceMinPriority(b) } }
    }; c.tryStrategy = function (a, b, c, e) { var f = null, g = null, g = a.connect(b, function (a, b) { if (!a || !f || !f.isRunning() || c.failFast) f && f.ensureAborted(), e(a, b) }); c.timeout > 0 && (f = new Pusher.Timer(c.timeout, function () { g.abort(); e(!0) })); return { abort: function () { f && f.ensureAborted(); g.abort() }, forceMinPriority: function (a) { g.forceMinPriority(a) } } };
    Pusher.SequentialStrategy = b
}).call(this);
(function () {
    function b(a, b, c, f) { this.name = a; this.priority = b; this.transport = c; this.options = f || {} } function c(a, b) { new Pusher.Timer(0, function () { b(a) }); return { abort: function () { }, forceMinPriority: function () { } } } var a = b.prototype; a.isSupported = function () { return this.transport.isSupported({ disableFlash: !!this.options.disableFlash }) }; a.connect = function (a, b) {
        if (this.transport.isSupported()) { if (this.priority < a) return c(new Pusher.Errors.TransportPriorityTooLow, b) } else return c(new Pusher.Errors.UnsupportedStrategy, b);
        var e = this, f = this.transport.createConnection(this.name, this.priority, this.options.key, this.options), g = function () { f.unbind("initialized", g); f.connect() }, i = function () { j(); b(null, f) }, k = function (a) { j(); b(a) }, l = function () { j(); b(new Pusher.Errors.TransportClosed(this.transport)) }, j = function () { f.unbind("initialized", g); f.unbind("open", i); f.unbind("error", k); f.unbind("closed", l) }; f.bind("initialized", g); f.bind("open", i); f.bind("error", k); f.bind("closed", l); f.initialize(); return {
            abort: function () {
                f.state !==
                "open" && (j(), f.close())
            }, forceMinPriority: function (a) { f.state !== "open" && e.priority < a && f.close() }
        }
    }; Pusher.TransportStrategy = b
}).call(this);
(function () {
    function b(a, b, c, f) { Pusher.EventsDispatcher.call(this); this.name = a; this.priority = b; this.key = c; this.state = "new"; this.timeline = f.timeline; this.id = this.timeline.generateUniqueID(); this.options = { encrypted: Boolean(f.encrypted), hostUnencrypted: f.hostUnencrypted, hostEncrypted: f.hostEncrypted } } function c(a) { return typeof a === "string" ? a : typeof a === "object" ? Pusher.Util.mapObject(a, function (a) { var b = typeof a; return b === "object" || b == "function" ? b : a }) : typeof a } var a = b.prototype; Pusher.Util.extend(a,
    Pusher.EventsDispatcher.prototype); b.isSupported = function () { return !1 }; a.supportsPing = function () { return !1 }; a.initialize = function () { this.timeline.info(this.buildTimelineMessage({ transport: this.name + (this.options.encrypted ? "s" : "") })); this.timeline.debug(this.buildTimelineMessage({ method: "initialize" })); this.changeState("initialized") }; a.connect = function () {
        var a = this.getURL(this.key, this.options); this.timeline.debug(this.buildTimelineMessage({ method: "connect", url: a })); if (this.socket || this.state !== "initialized") return !1;
        this.socket = this.createSocket(a); this.bindListeners(); Pusher.debug("Connecting", { transport: this.name, url: a }); this.changeState("connecting"); return !0
    }; a.close = function () { this.timeline.debug(this.buildTimelineMessage({ method: "close" })); return this.socket ? (this.socket.close(), !0) : !1 }; a.send = function (a) { this.timeline.debug(this.buildTimelineMessage({ method: "send", data: a })); if (this.state === "open") { var b = this; setTimeout(function () { b.socket.send(a) }, 0); return !0 } else return !1 }; a.requestPing = function () { this.emit("ping_request") };
    a.onOpen = function () { this.changeState("open"); this.socket.onopen = void 0 }; a.onError = function (a) { this.emit("error", { type: "WebSocketError", error: a }); this.timeline.error(this.buildTimelineMessage({ error: c(a) })) }; a.onClose = function (a) { this.changeState("closed", a); this.socket = void 0 }; a.onMessage = function (a) { this.timeline.debug(this.buildTimelineMessage({ message: a.data })); this.emit("message", a) }; a.bindListeners = function () {
        var a = this; this.socket.onopen = function () { a.onOpen() }; this.socket.onerror = function (b) { a.onError(b) };
        this.socket.onclose = function (b) { a.onClose(b) }; this.socket.onmessage = function (b) { a.onMessage(b) }
    }; a.createSocket = function () { return null }; a.getScheme = function () { return this.options.encrypted ? "wss" : "ws" }; a.getBaseURL = function () { var a; a = this.options.encrypted ? this.options.hostEncrypted : this.options.hostUnencrypted; return this.getScheme() + "://" + a }; a.getPath = function () { return "/app/" + this.key }; a.getQueryString = function () { return "?protocol=" + Pusher.PROTOCOL + "&client=js&version=" + Pusher.VERSION }; a.getURL = function () {
        return this.getBaseURL() +
        this.getPath() + this.getQueryString()
    }; a.changeState = function (a, b) { this.state = a; this.timeline.info(this.buildTimelineMessage({ state: a, params: b })); this.emit(a, b) }; a.buildTimelineMessage = function (a) { return Pusher.Util.extend({ cid: this.id }, a) }; Pusher.AbstractTransport = b
}).call(this);
(function () {
    function b(a, b, c, e) { Pusher.AbstractTransport.call(this, a, b, c, e) } var c = b.prototype; Pusher.Util.extend(c, Pusher.AbstractTransport.prototype); b.createConnection = function (a, c, h, e) { return new b(a, c, h, e) }; b.isSupported = function (a) { if (a && a.disableFlash) return !1; try { return !!new ActiveXObject("ShockwaveFlash.ShockwaveFlash") } catch (b) { return Boolean(navigator && navigator.mimeTypes && navigator.mimeTypes["application/x-shockwave-flash"] !== void 0) } }; c.initialize = function () {
        var a = this; this.timeline.info(this.buildTimelineMessage({
            transport: this.name +
            (this.options.encrypted ? "s" : "")
        })); this.timeline.debug(this.buildTimelineMessage({ method: "initialize" })); this.changeState("initializing"); if (window.WEB_SOCKET_SUPPRESS_CROSS_DOMAIN_SWF_ERROR === void 0) window.WEB_SOCKET_SUPPRESS_CROSS_DOMAIN_SWF_ERROR = !0; window.WEB_SOCKET_SWF_LOCATION = Pusher.Dependencies.getRoot() + "/WebSocketMain.swf"; Pusher.Dependencies.load("flashfallback", function () { a.changeState("initialized") })
    }; c.createSocket = function (a) { return new FlashWebSocket(a) }; c.getQueryString = function () {
        return Pusher.AbstractTransport.prototype.getQueryString.call(this) +
        "&flash=true"
    }; Pusher.FlashTransport = b
}).call(this);
(function () {
    function b(a, b, c, e) { Pusher.AbstractTransport.call(this, a, b, c, e) } var c = b.prototype; Pusher.Util.extend(c, Pusher.AbstractTransport.prototype); b.createConnection = function (a, c, h, e) { return new b(a, c, h, e) }; b.isSupported = function () { return !0 }; c.initialize = function () {
        var a = this; this.timeline.info(this.buildTimelineMessage({ transport: this.name + (this.options.encrypted ? "s" : "") })); this.timeline.debug(this.buildTimelineMessage({ method: "initialize" })); this.changeState("initializing"); Pusher.Dependencies.load("sockjs",
        function () { a.changeState("initialized") })
    }; c.supportsPing = function () { return !0 }; c.createSocket = function (a) { return new SockJS(a, null, { js_path: Pusher.Dependencies.getPath("sockjs", { encrypted: this.options.encrypted }) }) }; c.getScheme = function () { return this.options.encrypted ? "https" : "http" }; c.getPath = function () { return "/pusher" }; c.getQueryString = function () { return "" }; c.onOpen = function () {
        this.socket.send(JSON.stringify({ path: Pusher.AbstractTransport.prototype.getPath.call(this) + Pusher.AbstractTransport.prototype.getQueryString.call(this) }));
        this.changeState("open"); this.socket.onopen = void 0
    }; Pusher.SockJSTransport = b
}).call(this);
(function () {
    function b(a, b, c, e) { Pusher.AbstractTransport.call(this, a, b, c, e) } var c = b.prototype; Pusher.Util.extend(c, Pusher.AbstractTransport.prototype); b.createConnection = function (a, c, h, e) { return new b(a, c, h, e) }; b.isSupported = function () { return window.WebSocket !== void 0 || window.MozWebSocket !== void 0 }; c.createSocket = function (a) { return new (window.WebSocket || window.MozWebSocket)(a) }; c.getQueryString = function () { return Pusher.AbstractTransport.prototype.getQueryString.call(this) + "&flash=false" }; Pusher.WSTransport =
    b
}).call(this);
(function () {
    function b(a, b, c) { this.manager = a; this.transport = b; this.minPingDelay = c.minPingDelay || 1E4; this.maxPingDelay = c.maxPingDelay || Pusher.activity_timeout; this.pingDelay = null } var c = b.prototype; c.createConnection = function (a, b, c, e) {
        var f = this.transport.createConnection(a, b, c, e), g = this, i = null, k = null, l = function () { f.unbind("open", l); i = Pusher.Util.now(); g.pingDelay && (k = setInterval(function () { k && f.requestPing() }, g.pingDelay)); f.bind("closed", j) }, j = function (a) {
            f.unbind("closed", j); k && (clearInterval(k), k =
            null); if (!a.wasClean && i && (a = Pusher.Util.now() - i, a < 2 * g.maxPingDelay)) g.manager.reportDeath(), g.pingDelay = Math.max(a / 2, g.minPingDelay)
        }; f.bind("open", l); return f
    }; c.isSupported = function (a) { return this.manager.isAlive() && this.transport.isSupported(a) }; Pusher.AssistantToTheTransportManager = b
}).call(this);
(function () { function b(a) { this.options = a || {}; this.livesLeft = this.options.lives || Infinity } var c = b.prototype; c.getAssistant = function (a) { return new Pusher.AssistantToTheTransportManager(this, a, { minPingDelay: this.options.minPingDelay, maxPingDelay: this.options.maxPingDelay }) }; c.isAlive = function () { return this.livesLeft > 0 }; c.reportDeath = function () { this.livesLeft -= 1 }; Pusher.TransportManager = b }).call(this);
(function () {
    function b(a) { return function (b) { return [a.apply(this, arguments), b] } } function c(a, b) { if (a.length === 0) return [[], b]; var e = d(a[0], b), h = c(a.slice(1), e[1]); return [[e[0]].concat(h[0]), h[1]] } function a(a, b) {
        if (typeof a[0] === "string" && a[0].charAt(0) === ":") {
            var e = b[a[0].slice(1)]; if (a.length > 1) { if (typeof e !== "function") throw "Calling non-function " + a[0]; var h = [Pusher.Util.extend({}, b)].concat(Pusher.Util.map(a.slice(1), function (a) { return d(a, Pusher.Util.extend({}, b))[0] })); return e.apply(this, h) } else return [e,
            b]
        } else return c(a, b)
    } function d(b, c) { if (typeof b === "string") { var d; if (typeof b === "string" && b.charAt(0) === ":") { d = c[b.slice(1)]; if (d === void 0) throw "Undefined symbol " + b; d = [d, c] } else d = [b, c]; return d } else if (typeof b === "object" && b instanceof Array && b.length > 0) return a(b, c); return [b, c] } var h = { ws: Pusher.WSTransport, flash: Pusher.FlashTransport, sockjs: Pusher.SockJSTransport }, e = {
        def: function (a, b, c) { if (a[b] !== void 0) throw "Redefining symbol " + b; a[b] = c; return [void 0, a] }, def_transport: function (a, b, c, d, e, j) {
            var m =
            h[c]; if (!m) throw new Pusher.Errors.UnsupportedTransport(c); c = Pusher.Util.extend({}, { key: a.key, encrypted: a.encrypted, timeline: a.timeline, disableFlash: a.disableFlash }, e); j && (m = j.getAssistant(m)); d = new Pusher.TransportStrategy(b, d, m, c); j = a.def(a, b, d)[1]; j.transports = a.transports || {}; j.transports[b] = d; return [void 0, j]
        }, transport_manager: b(function (a, b) { return new Pusher.TransportManager(b) }), sequential: b(function (a, b) {
            var c = Array.prototype.slice.call(arguments, 2); return new Pusher.SequentialStrategy(c,
            b)
        }), cached: b(function (a, b, c) { return new Pusher.CachedStrategy(c, a.transports, { ttl: b, timeline: a.timeline }) }), first_connected: b(function (a, b) { return new Pusher.FirstConnectedStrategy(b) }), best_connected_ever: b(function () { var a = Array.prototype.slice.call(arguments, 1); return new Pusher.BestConnectedEverStrategy(a) }), delayed: b(function (a, b, c) { return new Pusher.DelayedStrategy(c, { delay: b }) }), "if": b(function (a, b, c, d) { return new Pusher.IfStrategy(b, c, d) }), is_supported: b(function (a, b) { return function () { return b.isSupported() } })
    };
    Pusher.StrategyBuilder = { build: function (a, b) { var c = Pusher.Util.extend({}, e, b); return d(a, c)[1].strategy } }
}).call(this);
(function () {
    function b(a) { Pusher.EventsDispatcher.call(this); this.transport = a; this.bindListeners() } var c = b.prototype; Pusher.Util.extend(c, Pusher.EventsDispatcher.prototype); c.supportsPing = function () { return this.transport.supportsPing() }; c.send = function (a) { return this.transport.send(a) }; c.send_event = function (a, b, c) { a = { event: a, data: b }; if (c) a.channel = c; Pusher.debug("Event sent", a); return this.send(JSON.stringify(a)) }; c.close = function () { this.transport.close() }; c.bindListeners = function () {
        var a = this, b =
        function (f) { f = a.parseMessage(f); if (f !== void 0) f.event === "pusher:connection_established" ? (a.id = f.data.socket_id, a.transport.unbind("message", b), a.transport.bind("message", c), a.transport.bind("ping_request", e), a.emit("connected", a.id)) : f.event === "pusher:error" && (a.handleCloseCode(f.data.code, f.data.message), a.transport.close()) }, c = function (b) {
            b = a.parseMessage(b); if (b !== void 0) {
                Pusher.debug("Event recd", b); switch (b.event) {
                    case "pusher:error": a.emit("error", { type: "PusherError", data: b.data }); break; case "pusher:ping": a.emit("ping");
                        break; case "pusher:pong": a.emit("pong")
                } a.emit("message", b)
            }
        }, e = function () { a.emit("ping_request") }, f = function (b) { a.emit("error", { type: "WebSocketError", error: b }) }, g = function (i) { i && i.code && a.handleCloseCode(i.code, i.reason); a.transport.unbind("message", b); a.transport.unbind("message", c); a.transport.unbind("ping_request", e); a.transport.unbind("error", f); a.transport.unbind("closed", g); a.transport = null; a.emit("closed") }; this.transport.bind("message", b); this.transport.bind("error", f); this.transport.bind("closed",
        g)
    }; c.parseMessage = function (a) { try { var b = JSON.parse(a.data); if (typeof b.data === "string") try { b.data = JSON.parse(b.data) } catch (c) { if (!(c instanceof SyntaxError)) throw c; } return b } catch (e) { this.emit("error", { type: "MessageParseError", error: e, data: a.data }) } }; c.handleCloseCode = function (a, b) {
        var c = !0; if (a < 4E3) { if (a === 1E3 || a === 1001) c = !1; a >= 1002 && a <= 1004 && this.emit("backoff") } else a === 4E3 ? this.emit("ssl_only") : a < 4100 ? this.emit("refused") : a < 4200 ? this.emit("backoff") : a < 4300 ? this.emit("retry") : this.emit("refused");
        c && this.emit("error", { type: "PusherError", data: { code: a, message: b } })
    }; Pusher.ProtocolWrapper = b
}).call(this);
(function () {
    function b(a, b) {
        Pusher.EventsDispatcher.call(this); this.key = a; this.options = b || {}; this.state = "initialized"; this.connection = null; this.encrypted = !!b.encrypted; this.timeline = this.options.getTimeline(); this.connectionCallbacks = this.buildCallbacks(); var c = this; Pusher.Network.bind("online", function () { c.state === "unavailable" && c.connect() }); Pusher.Network.bind("offline", function () { c.shouldRetry() && (c.disconnect(), c.updateState("unavailable")) }); var e = function () { c.timelineSender && c.timelineSender.send(function () { }) };
        this.bind("connected", e); setInterval(e, 6E4); this.updateStrategy()
    } var c = b.prototype; Pusher.Util.extend(c, Pusher.EventsDispatcher.prototype); c.connect = function () {
        if (!this.connection && this.state !== "connecting") if (this.strategy.isSupported()) if (Pusher.Network.isOnline() === !1) this.updateState("unavailable"); else {
            this.updateState("connecting"); this.timelineSender = this.options.getTimelineSender(this.timeline, { encrypted: this.encrypted }, this); var a = this, b = function (c, e) {
                c ? a.runner = a.strategy.connect(0, b) :
                (a.runner.abort(), a.setConnection(a.wrapTransport(e)))
            }; this.runner = this.strategy.connect(0, b); this.setUnavailableTimer()
        } else this.updateState("failed")
    }; c.send = function (a) { return this.connection ? this.connection.send(a) : !1 }; c.send_event = function (a, b, c) { return this.connection ? this.connection.send_event(a, b, c) : !1 }; c.disconnect = function () {
        this.runner && this.runner.abort(); this.clearRetryTimer(); this.clearUnavailableTimer(); this.stopActivityCheck(); this.updateState("disconnected"); this.connection && (this.connection.close(),
        this.abandonConnection())
    }; c.updateStrategy = function () { this.strategy = this.options.getStrategy({ key: this.key, timeline: this.timeline, encrypted: this.encrypted }) }; c.retryIn = function (a) { var b = this; this.retryTimer = setTimeout(function () { if (b.retryTimer !== null) b.retryTimer = null, b.disconnect(), b.connect() }, a || 0) }; c.clearRetryTimer = function () { if (this.retryTimer) clearTimeout(this.retryTimer), this.retryTimer = null }; c.setUnavailableTimer = function () {
        var a = this; this.unavailableTimer = setTimeout(function () {
            if (a.unavailableTimer) a.updateState("unavailable"),
            a.unavailableTimer = null
        }, this.options.unavailableTimeout)
    }; c.clearUnavailableTimer = function () { if (this.unavailableTimer) clearTimeout(this.unavailableTimer), this.unavailableTimer = null }; c.resetActivityCheck = function () { this.stopActivityCheck(); if (!this.connection.supportsPing()) { var a = this; this.activityTimer = setTimeout(function () { a.send_event("pusher:ping", {}); a.activityTimer = setTimeout(function () { a.connection.close() }, a.options.pongTimeout) }, this.options.activityTimeout) } }; c.stopActivityCheck = function () {
        if (this.activityTimer) clearTimeout(this.activityTimer),
        this.activityTimer = null
    }; c.buildCallbacks = function () {
        var a = this; return {
            connected: function (b) { a.clearUnavailableTimer(); a.socket_id = b; a.updateState("connected"); a.resetActivityCheck() }, message: function (b) { a.resetActivityCheck(); a.emit("message", b) }, ping: function () { a.send_event("pusher:pong", {}) }, ping_request: function () { a.send_event("pusher:ping", {}) }, error: function (b) { a.emit("error", { type: "WebSocketError", error: b }) }, closed: function () { a.abandonConnection(); a.shouldRetry() && a.retryIn(1E3) }, ssl_only: function () {
                a.encrypted =
                !0; a.updateStrategy(); a.retryIn(0)
            }, refused: function () { a.disconnect() }, backoff: function () { a.retryIn(1E3) }, retry: function () { a.retryIn(0) }
        }
    }; c.setConnection = function (a) { this.connection = a; for (var b in this.connectionCallbacks) this.connection.bind(b, this.connectionCallbacks[b]); this.resetActivityCheck() }; c.abandonConnection = function () { for (var a in this.connectionCallbacks) this.connection.unbind(a, this.connectionCallbacks[a]); this.connection = null }; c.updateState = function (a, b) {
        var c = this.state; this.state =
        a; c !== a && (Pusher.debug("State changed", c + " -> " + a), this.emit("state_change", { previous: c, current: a }), this.emit(a, b))
    }; c.shouldRetry = function () { return this.state === "connecting" || this.state === "connected" }; c.wrapTransport = function (a) { return new Pusher.ProtocolWrapper(a) }; Pusher.ConnectionManager = b
}).call(this);
(function () { function b() { Pusher.EventsDispatcher.call(this); var b = this; window.addEventListener !== void 0 && (window.addEventListener("online", function () { b.emit("online") }, !1), window.addEventListener("offline", function () { b.emit("offline") }, !1)) } Pusher.Util.extend(b.prototype, Pusher.EventsDispatcher.prototype); b.prototype.isOnline = function () { return window.navigator.onLine === void 0 ? !0 : window.navigator.onLine }; Pusher.NetInfo = b; Pusher.Network = new b }).call(this);
(function () {
    Pusher.Channels = function () { this.channels = {} }; Pusher.Channels.prototype = { add: function (b, a) { var d = this.find(b); d || (d = Pusher.Channel.factory(b, a), this.channels[b] = d); return d }, find: function (b) { return this.channels[b] }, remove: function (b) { delete this.channels[b] }, disconnect: function () { for (var b in this.channels) this.channels[b].disconnect() } }; Pusher.Channel = function (b, a) {
        var d = this; Pusher.EventsDispatcher.call(this, function (a) { Pusher.debug("No callbacks on " + b + " for " + a) }); this.pusher = a; this.name =
        b; this.subscribed = !1; this.bind("pusher_internal:subscription_succeeded", function (a) { d.onSubscriptionSucceeded(a) })
    }; Pusher.Channel.prototype = { init: function () { }, disconnect: function () { this.subscribed = !1; this.emit("pusher_internal:disconnected") }, onSubscriptionSucceeded: function () { this.subscribed = !0; this.emit("pusher:subscription_succeeded") }, authorize: function (b, a, d) { return d(!1, {}) }, trigger: function (b, a) { return this.pusher.send_event(b, a, this.name) } }; Pusher.Util.extend(Pusher.Channel.prototype, Pusher.EventsDispatcher.prototype);
    Pusher.Channel.PrivateChannel = { authorize: function (b, a, d) { var h = this; return (new Pusher.Channel.Authorizer(this, Pusher.channel_auth_transport, a)).authorize(b, function (a, b) { a || h.emit("pusher_internal:authorized", b); d(a, b) }) } }; Pusher.Channel.PresenceChannel = { init: function () { this.members = new b(this) }, onSubscriptionSucceeded: function () { this.subscribed = !0 } }; var b = function (b) {
        var a = this, d = null, h = function () { a._members_map = {}; a.count = 0; d = a.me = null }; h(); var e = function (f) {
            a._members_map = f.presence.hash; a.count =
            f.presence.count; a.me = a.get(d.user_id); b.emit("pusher:subscription_succeeded", a)
        }; b.bind("pusher_internal:authorized", function (a) { d = JSON.parse(a.channel_data); b.bind("pusher_internal:subscription_succeeded", e) }); b.bind("pusher_internal:member_added", function (d) { a.get(d.user_id) === null && a.count++; a._members_map[d.user_id] = d.user_info; b.emit("pusher:member_added", a.get(d.user_id)) }); b.bind("pusher_internal:member_removed", function (d) {
            var e = a.get(d.user_id); e && (delete a._members_map[d.user_id], a.count--,
            b.emit("pusher:member_removed", e))
        }); b.bind("pusher_internal:disconnected", function () { h(); b.unbind("pusher_internal:subscription_succeeded", e) })
    }; b.prototype = { each: function (b) { for (var a in this._members_map) b(this.get(a)) }, get: function (b) { return this._members_map.hasOwnProperty(b) ? { id: b, info: this._members_map[b] } : null } }; Pusher.Channel.factory = function (b, a) {
        var d = new Pusher.Channel(b, a); b.indexOf("private-") === 0 ? Pusher.Util.extend(d, Pusher.Channel.PrivateChannel) : b.indexOf("presence-") === 0 && (Pusher.Util.extend(d,
        Pusher.Channel.PrivateChannel), Pusher.Util.extend(d, Pusher.Channel.PresenceChannel)); d.init(); return d
    }
}).call(this);
(function () {
    Pusher.Channel.Authorizer = function (b, c, a) { this.channel = b; this.type = c; this.authOptions = (a || {}).auth || {} }; Pusher.Channel.Authorizer.prototype = { composeQuery: function (b) { var b = "&socket_id=" + encodeURIComponent(b) + "&channel_name=" + encodeURIComponent(this.channel.name), c; for (c in this.authOptions.params) b += "&" + encodeURIComponent(c) + "=" + encodeURIComponent(this.authOptions.params[c]); return b }, authorize: function (b, c) { return Pusher.authorizers[this.type].call(this, b, c) } }; Pusher.auth_callbacks = {};
    Pusher.authorizers = {
        ajax: function (b, c) {
            var a; a = Pusher.XHR ? new Pusher.XHR : window.XMLHttpRequest ? new window.XMLHttpRequest : new ActiveXObject("Microsoft.XMLHTTP"); a.open("POST", Pusher.channel_auth_endpoint, !0); a.setRequestHeader("Content-Type", "application/x-www-form-urlencoded"); for (var d in this.authOptions.headers) a.setRequestHeader(d, this.authOptions.headers[d]); a.onreadystatechange = function () {
                if (a.readyState == 4) if (a.status == 200) {
                    var b, d = !1; try { b = JSON.parse(a.responseText), d = !0 } catch (f) {
                        c(!0, "JSON returned from webapp was invalid, yet status code was 200. Data was: " +
                        a.responseText)
                    } d && c(!1, b)
                } else Pusher.warn("Couldn't get auth info from your webapp", a.status), c(!0, a.status)
            }; a.send(this.composeQuery(b)); return a
        }, jsonp: function (b, c) {
            this.authOptions.headers !== void 0 && Pusher.warn("Warn", "To send headers with the auth request, you must use AJAX, rather than JSONP."); var a = document.createElement("script"); Pusher.auth_callbacks[this.channel.name] = function (a) { c(!1, a) }; a.src = Pusher.channel_auth_endpoint + "?callback=" + encodeURIComponent("Pusher.auth_callbacks['" + this.channel.name +
            "']") + this.composeQuery(b); var d = document.getElementsByTagName("head")[0] || document.documentElement; d.insertBefore(a, d.firstChild)
        }
    }
}).call(this);