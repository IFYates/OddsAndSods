/*--------------------------------------------------------------------------
* Snippet to combine Linq.js and Knockout.js
* ver 1.0.0.0 2015-04-15
*
* created and maintained by Ian Yates
* licensed under Microsoft Public License(Ms-PL)
* https://github.com/IanYates83/
*--------------------------------------------------------------------------*/

(function () {
	// Iterate through all Enumerable public methods
	for (var k in Enumerable.prototype) {
		if (Enumerable.prototype.hasOwnProperty(k) && typeof Enumerable.prototype[k] === 'function') {
			// Apply non-overwriting methods to computed
			if (!(k in ko.computed)) {
				(function (nm) {
					ko.computed.fn[nm] = function () {
						var e = Enumerable.From(this());
						return e[nm].apply(e, arguments);
					};
				})(k);
			}

			// Apply non-overwriting methods to observable
			if (!(k in ko.observable)) {
				(function (nm) {
					ko.observable.fn[nm] = function () {
						var e = Enumerable.From(this());
						return e[nm].apply(e, arguments);
					};
				})(k);
			}

			// Apply non-overwriting methods to observableArray
			if (!(k in ko.observableArray)) {
				(function (nm) {
					ko.observableArray.fn[nm] = function () {
						var e = Enumerable.From(this());
						return e[nm].apply(e, arguments);
					};
				})(k);
			}
		}
	}
})();