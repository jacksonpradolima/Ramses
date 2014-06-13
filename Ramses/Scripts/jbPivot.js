$.unc = {
    plugins: {
        agregates: {},
        addAgregate: function (b, a) {
            this.agregates[b] = a
        },
        groupers: {},
        addGrouper: function (b, a) {
            this.groupers[b] = a
        },
        formatters: {},
        addFormatter: function (b, a) {
            this.formatters[b] = a
        }
    }
};
$(function () {
    $.widget("unc.jbPivot", {
        options: {
            fields: {},
            summary: true,
            copyright: true,
            formatter: "default"
        },
        _create: function () {
            this.CollapsedNodes = {};
            this.renderPending = false;
            this.reset();
            if (typeof this.options.data !== "undefined") {
                this.insertRecords(this.options.data)
            }
            var a = this;
            $(this.element).on("click", ".collapse_button", function (b) {
                b.preventDefault();
                a._togleCollapse(this.rel)
            });
            $(this.element).on("dragstart", ".draggable", function (b) {
                a.pre_post = "";
                a.dragcls = "";
                a.dragdata = $(this).attr("rel");
                b.originalEvent.dataTransfer.effectAllowed = "move";
                b.originalEvent.dataTransfer.setData("Text", $(this).attr("rel"));
                $(".pivot").addClass("drag_in_progress")
            });
            $(this.element).on("dragleave", ".dropable", function (e) {
                var f = $(this);
                var b = f.attr("class").match(/\s+(target(X|Y|Z|U)[\d]+)($|\s)/);
                var c = "." + b[1];
                var d = a.element;
                setTimeout(function () {
                    $(c, d).removeClass("dropping_pre");
                    $(c, d).removeClass("dropping_post");
                    a.pre_post = "";
                    a.dragcls = ""
                }, 0);
                e.stopPropagation();
                e.preventDefault();
                return false
            });
            $(this.element).on("dragover", ".dropable", function (d) {
                var k = $(this);
                var b = $(this).offset();
                var h = d.originalEvent.pageX - b.left;
                var g = d.originalEvent.pageY - b.top;
                var i = k.attr("class").match(/\s+(target(X|Y|Z|U)([\d]+))($|\s)/);
                i[3] = parseInt(i[3], 10);
                var n = "." + i[1];
                var f;
                var c = a.pre_post;
                var j = a.dragcls;
                if ((i[2] === "Y") || (i[2] === "U")) {
                    f = g > k.height() / 2 ? "post" : "pre"
                } else {
                    f = h > k.width() / 2 ? "post" : "pre"
                } if (i[3] === 0) {
                    f = "post"
                }
                var m = a.dragdata;
                var l = $(this).attr("rel");
                if (a._canMove(m, l, f)) {
                    var e = a.element;
                    if ((c !== f) || (n !== j)) {
                        $(n, e).removeClass("dropping_pre");
                        $(n, e).removeClass("dropping_post");
                        setTimeout(function () {
                            $(n, e).addClass("dropping_" + f);
                            a.pre_post = f;
                            a.dragcls = n
                        }, 0)
                    }
                    d.originalEvent.dataTransfer.dropEffect = "move";
                    d.stopPropagation();
                    d.preventDefault();
                    return false
                }
            });
            $(this.element).on("drop", ".dropable", function (c) {
                var g = $(this);
                var b = $(this).offset();
                var e = c.originalEvent.pageX - b.left;
                var d = c.originalEvent.pageY - b.top;
                var f = g.attr("class").match(/\s+(target(X|Y|Z|U)([\d]+))($|\s)/);
                f[3] = parseInt(f[3], 10);
                var j;
                if ((f[2] === "Y") || (f[2] === "U")) {
                    j = d > g.height() / 2 ? "post" : "pre"
                } else {
                    j = e > g.width() / 2 ? "post" : "pre"
                } if (f[3] === 0) {
                    j = "post"
                }
                var i = c.originalEvent.dataTransfer.getData("Text");
                var h = $(this).attr("rel");
                a._moveField(i, h, j);
                c.preventDefault();
                return false
            });
            $(this.element).on("dragend", ".dropable", function () {
                var b = a.element;
                $(".pivot", b).removeClass("drag_in_progress")
            })
        },
        reset: function () {
            this.fields = [];
            this.afields = [];
            this.xfields = [];
            this.yfields = [];
            this.zfields = [];
            this.ufields = [];
            this.fieldNames = [];
            this.fieldLabels = [];
            this.formatters = [];
            if ((typeof this.options.formatter === "undefined") || (this.options.formatter === null)) {
                this.options.formatter = "default"
            }
            if (typeof this.options.formatter === "string") {
                if (typeof $.unc.plugins.formatters[this.options.formatter] !== "function") {
                    throw ("Formatter: " + this.options.formatter + " is not defined")
                }
                this.defaultFormatter = $.unc.plugins.formatters[this.options.formatter](this.options)
            } else {
                if (typeof this.options.formatter === "function") {
                    this.defaultFormatter = {
                        format: this.options.formatter
                    }
                } else {
                    throw ("Invalid formatter")
                }
            } if (this.options.formatter === null) {
                throw ("Formatter not defined")
            }
            var h;
            var b = 0;
            var g = {};
            for (h in this.options.fields) {
                if (this.options.fields.hasOwnProperty(h)) {
                    var d = $.extend({
                        agregateType: "distinct",
                        groupType: "distinct",
                        label: h,
                        formatter: null
                    }, this.options.fields[h]);
                    if (d.groupType !== "none") {
                        if (typeof $.unc.plugins.groupers[d.groupType] !== "function") {
                            throw ("Grouper: " + d.groupType + " is not defined")
                        }
                        this.fields.push($.unc.plugins.groupers[d.groupType](d))
                    } else {
                        this.fields.push(null)
                    } if (d.agregateType !== "none") {
                        if (typeof $.unc.plugins.agregates[d.agregateType] !== "function") {
                            throw ("Grouper: " + d.agregateType + " is not defined")
                        }
                        this.afields.push($.unc.plugins.agregates[d.agregateType](d))
                    } else {
                        this.afields.push(null)
                    } if (typeof d.formatter === "string") {
                        if (typeof $.unc.plugins.formatters[d.formatter] !== "function") {
                            throw ("Formatter" + d.formatter + " is not defined")
                        }
                        this.formatters.push($.unc.plugins.formatters[d.formatter](d))
                    } else {
                        if (typeof d.formatter === "function") {
                            this.formatters.push({
                                format: d.formatter
                            })
                        } else {
                            this.formatters.push(null)
                        }
                    }
                    g[h] = b;
                    b++;
                    this.fieldNames.push(h);
                    this.fieldLabels.push(d.label)
                }
            }
            var c, e;
            for (c = 0; c < this.options.xfields.length; c++) {
                e = this.options.xfields[c];
                if (typeof g[e] === "undefined") {
                    throw ("Field " + e + " in xfields not defined")
                }
                if (typeof this.fields[g[e]] === null) {
                    throw ("Field " + e + "is not groupable ans is ispecified in xfields")
                }
                this.xfields.push(g[e]);
                delete g[e]
            }
            for (c = 0; c < this.options.yfields.length; c++) {
                e = this.options.yfields[c];
                if (typeof g[e] === "undefined") {
                    throw ("Field " + e + " in yfields not defined")
                }
                if (typeof this.fields[g[e]] === null) {
                    throw ("Field " + e + "is not groupable ans is ispecified in yfields")
                }
                this.yfields.push(g[e]);
                delete g[e]
            }
            for (c = 0; c < this.options.zfields.length; c++) {
                e = this.options.zfields[c];
                if (typeof g[e] === "undefined") {
                    throw ("Field " + e + " in zfields not defined")
                }
                if (typeof this.afields[g[e]] === null) {
                    throw ("Field " + e + "is not agregatable ans is ispecified in zfields")
                }
                this.zfields.push(g[e]);
                delete g[e]
            }
            for (e in g) {
                if (g.hasOwnProperty(e)) {
                    this.ufields.push(g[e])
                }
            }
            var a = (1 << this.fields.length) - 1;
            this.indexes = {};
            this.indexes_len = {};
            this.indexes[a] = {};
            this.indexes_len[a] = 0;
            this._generate_trees();
            this._forceRender()
        },
        insertRecords: function (a) {
            var d, e;
            for (d = 0; d < a.length; d++) {
                var c = a[d];
                var j = [];
                for (e = 0; e < this.fields.length; e++) {
                    if (this.fields[e] !== null) {
                        j.push(this.fields[e].CalculateValue(c))
                    } else {
                        j.push(0)
                    }
                }
                for (var l in this.indexes) {
                    if (this.indexes.hasOwnProperty(l)) {
                        var g = j.slice(0);
                        for (var b = 0; b < this.fields.length; b++) {
                            if (((1 << b) & l) === 0) {
                                g[b] = 0
                            }
                        }
                        var h = this._arr2idx(g);
                        if (typeof this.indexes[l][h] === "undefined") {
                            this.indexes[l][h] = [];
                            this.indexes_len[l]++
                        }
                        for (e = 0; e < this.afields.length; e++) {
                            if (this.afields[e] !== null) {
                                this.indexes[l][h][e] = this.afields[e].agregate(this.indexes[l][h][e], c)
                            }
                        }
                    }
                }
            }
            this._generate_trees();
            this._forceRender()
        },
        _forceRender: function () {
            if (this.renderPending) {
                return
            }
            this.renderPending = true;
            var a = this;
            setTimeout(function () {
                a.renderPending = false;
                a._renderHtml()
            }, 100)
        },
        _arr2idx: function (a) {
            return a.join(",")
        },
        _idx2arr: function (d) {
            var c;
            var a = d.split(",");
            var b = [];
            for (c = 0; c < a.length; c++) {
                b.push(parseInt(a[c], 10))
            }
            return b
        },
        _getValues: function (e) {
            var c;
            var b = 0;
            for (var d = 0; d < this.fields.length; d++) {
                if (e[d] === -1) {
                    e[d] = 0
                } else {
                    b = b | (1 << d)
                }
            }
            if (typeof this.indexes[b] === "undefined") {
                this._generate_index(b)
            }
            c = this._arr2idx(e);
            var a = this.indexes[b][c];
            if ((typeof a === "undefined") || (a === null)) {
                a = new Array(this.afields.length)
            }
            return a
        },
        _generate_index: function (b) {
            var a;
            var h = null;
            for (a in this.indexes) {
                if ((b & a) === b) {
                    if ((h === null) || (this.indexes_len[a] < this.indexes_len[h])) {
                        h = a
                    }
                }
            }
            this.indexes[b] = {};
            this.indexes_len[b] = 0;
            for (var d in this.indexes[h]) {
                if (this.indexes[h].hasOwnProperty(d)) {
                    var e = this._idx2arr(d);
                    for (a = 0; a < this.fields.length; a++) {
                        if (((1 << a) & b) === 0) {
                            e[a] = 0
                        }
                    }
                    var c = this._arr2idx(e);
                    if (typeof this.indexes[b][c] === "undefined") {
                        this.indexes[b][c] = [];
                        this.indexes_len[b]++
                    }
                    for (var g = 0; g < this.afields.length; g++) {
                        if (this.afields[g] !== null) {
                            this.indexes[b][c][g] = this.afields[g].agregate(this.indexes[b][c][g], this.indexes[h][d][g])
                        }
                    }
                }
            }
        },
        _generate_trees: function () {
            var e, c;
            var a = (1 << this.fields.length) - 1;
            this.xtree = {
                childs: {}
            };
            this.ytree = {
                childs: {}
            };
            for (var b in this.indexes[a]) {
                if (this.indexes[a].hasOwnProperty(b)) {
                    var d = this._idx2arr(b);
                    e = this.xtree;
                    for (c = 0; c < this.xfields.length; c++) {
                        if (typeof e.childs[d[this.xfields[c]]] === "undefined") {
                            e.childs[d[this.xfields[c]]] = {
                                childs: {}
                            }
                        }
                        e = e.childs[d[this.xfields[c]]]
                    }
                    e = this.ytree;
                    for (c = 0; c < this.yfields.length; c++) {
                        if (typeof e.childs[d[this.yfields[c]]] === "undefined") {
                            e.childs[d[this.yfields[c]]] = {
                                childs: {}
                            }
                        }
                        e = e.childs[d[this.yfields[c]]]
                    }
                }
            }
        },
        _tree2table: function (e, d, o) {
            var j, i, h;
            var k = e === "y" ? this.yfields.length : this.xfields.length;
            i = o.length - 1;
            j = o[i].index.length;
            var f;
            if (this._isCollapsed(e, o[i].index)) {
                o[i].cells[j] = {};
                o[i].cells[j].spanx = k - j + 1;
                o[i].cells[j].spany = 1;
                f = 1
            } else {
                var p = o[i].index.slice(0);
                var b = false;
                f = 0;
                var n;
                if (j < k) {
                    var l = [];
                    for (h in d.childs) {
                        if (d.childs.hasOwnProperty(h)) {
                            l.push(h)
                        }
                    }
                    var a = e === "y" ? this.fields[this.yfields[j]] : this.fields[this.xfields[j]];
                    n = a.DisplayValues(l)
                } else {
                    n = []
                }
                for (h = 0; h < n.length; h++) {
                    if (b) {
                        o.push({
                            index: p.slice(0),
                            cells: {}
                        })
                    }
                    o[o.length - 1].index.push(n[h]);
                    var g;
                    if (typeof d.childs[n[h]] === "undefined") {
                        g = {
                            childs: {},
                            collapsed: false
                        }
                    } else {
                        g = d.childs[n[h]]
                    }
                    f += this._tree2table(e, g, o);
                    b = true
                }
                if (f === 0) {
                    o[i].cells[j] = {};
                    o[i].cells[j].spanx = k - j + 1;
                    o[i].cells[j].spany = 1;
                    f = 1
                } else {
                    o[i].cells[j] = {};
                    o[i].cells[j].spanx = 1;
                    o[i].cells[j].spany = f
                }
            }
            return f
        },
        _collapseLink: function (b, c) {
            var a = '<A href="#" class="collapse_button" rel="' + this._treeNode2str(b, c) + '">';
            a += this._isCollapsed(b, c) ? "+" : "-";
            a += "</A>";
            return a
        },
        _renderHtml: function () {
            var a, m, d, j, h, g, n;
            var l = this.xfields.length > this.yfields.length ? this.xfields.length : this.yfields.length;
            l++;
            var f = [{
                index: [],
                cells: {}
            }];
            this._tree2table("x", this.xtree, f);
            var e = [{
                index: [],
                cells: {}
            }];
            this._tree2table("y", this.ytree, e);
            var c = "";
            c += "<table border='0px' cellspacing='0' cellpadding='0' class='unused_fields'><tr><th class='unused_field dropable targetU0' rel='U,0'>Outros campos</th>";
            for (d = 0; d < this.ufields.length; d++) {
                c += "<tr><td draggable='true'";
                m = "unused_field draggable dropable";
                m += " targetU" + (d + 1);
                c += ' class="' + m + '"';
                c += " rel= 'U," + (d + 1) + "'";
                c += ">";
                c += this.fieldNames[this.ufields[d]];
                c += "</td></tr>"
            }
            c += "</table>";
            c += '<table border="0px" cellspacing="0" cellpadding="0" class="pivot">';
            c += "<tr>";
            c += '<th colspan="' + (this.xfields.length + 1) + '" rowspan="' + (this.yfields.length + 2) + '"';
            m = "";
            m += " line_bottom_" + l;
            m += " line_right_" + l;
            c += ' class="' + m + '"';
            c += ">";
            c += "</th>";
            a = e.length * (this.zfields.length > 0 ? this.zfields.length : 1);
            c += "<th colspan=" + a + '"';
            m = "dropable toptitle targetY0 line_top_" + l;
            m += " line_left_" + l;
            m += " line_right_" + l;
            c += ' class="' + m + '"';
            c += ' rel="Y,0"';
            c += ">";
            if (this.yfields.length > 0) {
                c += this._collapseLink("y", [])
            }
            c += "Tudo";
            c += "</th>";
            c += "</tr>\n";
            for (h = 1; h <= this.yfields.length; h++) {
                c += "<tr>";
                for (j = 0; j < e.length; j++) {
                    if (e[j].cells[h]) {
                        c += '<th draggable="true"';
                        m = "draggable dropable toptitle";
                        m += this._clsLeftLine(e, j, e[j].cells[h].spany);
                        m += this._clsRightLine(e, j, e[j].cells[h].spany);
                        m += " targetY" + h;
                        c += ' class="' + m + '"';
                        c += ' rel= "Y,' + h + '"';
                        a = e[j].cells[h].spany * (this.zfields.length > 0 ? this.zfields.length : 1);
                        c += ' colspan="' + a + '"';
                        c += ' rowspan="' + e[j].cells[h].spanx + '"';
                        c += ">";
                        if (h < this.yfields.length) {
                            c += this._collapseLink("y", e[j].index.slice(0, h))
                        }
                        c += this.fields[this.yfields[h - 1]].getStringValue(e[j].index[h - 1]);
                        c += "</th>"
                    }
                }
                c += "</tr>\n"
            }
            c += "<tr>";
            for (j = 0; j < e.length; j++) {
                for (g = 0; g < this.zfields.length; g++) {
                    c += '<th draggable="true"';
                    m = "draggable dropable ztitle";
                    m += " line_top_0";
                    m += " targetZ" + (g + 1);
                    m += " line_top_" + l;
                    if (g === 0) {
                        m += this._clsLeftLine(e, j)
                    } else {
                        m += " line_left_0"
                    } if (g === this.zfields.length - 1) {
                        m += this._clsRightLine(e, j)
                    } else {
                        m += " line_right_0"
                    }
                    c += ' class="' + m + '"';
                    c += ' rel= "Z,' + (g + 1) + '"';
                    c += ">";
                    c += this.fieldLabels[this.zfields[g]];
                    c += "</th>"
                }
                if (this.zfields.length === 0) {
                    c += "<th";
                    m = "dropable ztitle";
                    m += " line_top_0";
                    m += " targetZ0";
                    m += " line_top_" + l;
                    m += this._clsLeftLine(e, j);
                    m += this._clsRightLine(e, j);
                    c += " class='" + m + "'";
                    c += " rel= 'Z,0'";
                    c += ">";
                    c += "&nbsp;";
                    c += "</th>"
                }
            }
            c += "<tr>";
            for (h = 0; h < f.length; h++) {
                for (j = 0; j <= this.xfields.length; j++) {
                    if (f[h].cells[j]) {
                        c += "<th";
                        c += f[h].cells[j].spanx > 1 ? ' colspan="' + f[h].cells[j].spanx + '"' : "";
                        c += f[h].cells[j].spany > 1 ? ' rowspan="' + f[h].cells[j].spany + '"' : "";
                        m = "lefttitle dropable";
                        if (j > 0) {
                            m += " draggable"
                        }
                        if (j === 0) {
                            m += " line_left_" + l
                        }
                        m += this._clsTopLine(f, h, f[h].cells[j].spany);
                        m += this._clsBottomLine(f, h, f[h].cells[j].spany);
                        m += " targetX" + j;
                        c += ' class="' + m + '"';
                        c += '"';
                        c += ' rel="X,' + j + '"';
                        if (j > 0) {
                            c += ' draggable="true"'
                        }
                        c += ">";
                        if (j < this.xfields.length) {
                            c += this._collapseLink("x", f[h].index.slice(0, j))
                        }
                        if (j === 0) {
                            c += "Tudo"
                        } else {
                            c += this.fields[this.xfields[j - 1]].getStringValue(f[h].index[j - 1])
                        }
                        c += "</th>"
                    }
                }
                for (j = 0; j < e.length; j++) {
                    n = [];
                    for (d = 0; d < this.fields.length; d++) {
                        n.push(-1)
                    }
                    for (d = 0; d < f[h].index.length; d++) {
                        n[this.xfields[d]] = f[h].index[d]
                    }
                    for (d = 0; d < e[j].index.length; d++) {
                        n[this.yfields[d]] = e[j].index[d]
                    }
                    b = this._getValues(n);
                    for (g = 0; g < this.zfields.length; g++) {
                        c += "<td";
                        m = "";
                        m += this._clsTopLine(f, h);
                        m += this._clsBottomLine(f, h);
                        if (g === 0) {
                            m += this._clsLeftLine(e, j)
                        } else {
                            m += " line_left_0"
                        } if (g === this.zfields.length - 1) {
                            m += this._clsRightLine(e, j)
                        } else {
                            m += " line_right_0"
                        }
                        c += ' class="' + m + '"';
                        c += ">";
                        c += this._format(this.afields[this.zfields[g]].getValue(b[this.zfields[g]]), this.zfields[g]);
                        c += "</td>"
                    }
                    if (this.zfields.length === 0) {
                        c += "<td";
                        m = "";
                        m += this._clsTopLine(f, h);
                        m += this._clsBottomLine(f, h);
                        m += this._clsLeftLine(e, j);
                        m += this._clsRightLine(e, j);
                        c += " class='" + m + "'";
                        c += ">";
                        c += "&nbsp;";
                        c += "</td>"
                    }
                }
                c += "</tr>"
            }
            if ((this.options.summary) && (this.zfields.length > 0)) {
                c += "<tr>";
                c += "<td colspan='" + (this.xfields.length + 1) + "'";
                c += " >";
                c += "</td>";
                for (j = 0; j < e.length; j++) {
                    n = [];
                    for (d = 0; d < this.fields.length; d++) {
                        n.push(-1)
                    }
                    for (d = 0; d < e[j].index.length; d++) {
                        n[this.yfields[d]] = e[j].index[d]
                    }
                    var b = this._getValues(n);
                    for (g = 0; g < this.zfields.length; g++) {
                        c += "<td";
                        m = "summary";
                        m += " line_top_" + l;
                        m += " line_bottom_" + l;
                        if (g === 0) {
                            m += this._clsLeftLine(e, j)
                        } else {
                            m += " line_left_0"
                        } if (g === this.zfields.length - 1) {
                            m += this._clsRightLine(e, j)
                        } else {
                            m += " line_right_0"
                        }
                        c += ' class="' + m + '"';
                        c += ">";
                        c += this._format(this.afields[this.zfields[g]].getValue(b[this.zfields[g]]), this.zfields[g]);
                        c += "</td>"
                    }
                }
                c += "</tr>"
            }
            var k = 1 + this.xfields.length + e.length * this.zfields.length;
            c += "<tr><td colspan='" + k;
            m = "";
            m += " line_right_" + l;
            m += " line_left_" + l;
            m += " line_top_" + l;
            c += " class='" + m + "'";
            c += ">";
            c += "<tr>";
            for (d = 0; d < k; d++) {
                c += "<td ";
                m = "bordermark";
                c += " class='" + m + "'";
                c += ">";
                c += "</td>"
            }
            c += "</tr>";       
            c += "</table>";
            this.element[0].innerHTML = c
        },
        _treeNode2str: function (c, d) {
            var b;
            var a = [];
            if (d.length === 0) {
                return c
            }
            for (b = 0; b < this.fields.length; b++) {
                a.push(-1)
            }
            for (b = 0; b < d.length; b++) {
                if (c === "y") {
                    a[this.yfields[b]] = d[b]
                } else {
                    a[this.xfields[b]] = d[b]
                }
            }
            return a.join(",")
        },
        _isCollapsed: function (b, c) {
            var a = this._treeNode2str(b, c);
            return (typeof this.CollapsedNodes[a] !== "undefined")
        },
        _togleCollapse: function (a) {
            if (typeof this.CollapsedNodes[a] === "undefined") {
                this.CollapsedNodes[a] = true
            } else {
                delete this.CollapsedNodes[a]
            }
            this._forceRender()
        },
        _canMove: function (g, e, c) {
            var d = g.split(",");
            d[1] = parseInt(d[1], 10);
            var b = e.split(",");
            b[1] = parseInt(b[1], 10);
            if (c === "pre") {
                b[1]--
            }
            d[1]--;
            var a = false;
            if (b[0] === "Z") {
                if (d[0] === "X") {
                    a = (this.afields[this.xfields[d[1]]] !== null)
                } else {
                    if (d[0] === "Y") {
                        a = (this.afields[this.yfields[d[1]]] !== null)
                    } else {
                        if (d[0] === "Z") {
                            a = (this.afields[this.zfields[d[1]]] !== null)
                        } else {
                            if (d[0] === "U") {
                                a = (this.afields[this.ufields[d[1]]] !== null)
                            }
                        }
                    }
                }
            } else {
                if ((b[0] === "X") || (b[0] === "Y")) {
                    if (d[0] === "X") {
                        a = (this.fields[this.xfields[d[1]]] !== null)
                    } else {
                        if (d[0] === "Y") {
                            a = (this.fields[this.yfields[d[1]]] !== null)
                        } else {
                            if (d[0] === "Z") {
                                a = (this.fields[this.zfields[d[1]]] !== null)
                            } else {
                                if (d[0] === "U") {
                                    a = (this.fields[this.ufields[d[1]]] !== null)
                                }
                            }
                        }
                    }
                } else {
                    if (b[0] === "U") {
                        a = true
                    }
                }
            }
            return a
        },
        _moveField: function (g, e, b) {
            var c = g.split(",");
            c[1] = parseInt(c[1], 10);
            var a = e.split(",");
            a[1] = parseInt(a[1], 10);
            if (b === "pre") {
                a[1]--
            }
            var d = -1;
            if (c[0] === "X") {
                d = this.xfields[c[1] - 1];
                this.xfields.splice(c[1] - 1, 1)
            } else {
                if (c[0] === "Y") {
                    d = this.yfields[c[1] - 1];
                    this.yfields.splice(c[1] - 1, 1)
                } else {
                    if (c[0] === "Z") {
                        d = this.zfields[c[1] - 1];
                        this.zfields.splice(c[1] - 1, 1)
                    } else {
                        if (c[0] === "U") {
                            d = this.ufields[c[1] - 1];
                            this.ufields.splice(c[1] - 1, 1)
                        }
                    }
                }
            } if (d === -1) {
                throw "Assert: " + c[0] + " is an invalid axis"
            }
            if ((c[0] === a[0]) && (c[1] < a[1])) {
                a[1]--
            }
            if (a[0] === "X") {
                this.xfields.splice(a[1], 0, d)
            } else {
                if (a[0] === "Y") {
                    this.yfields.splice(a[1], 0, d)
                } else {
                    if (a[0] === "Z") {
                        this.zfields.splice(a[1], 0, d)
                    } else {
                        if (a[0] === "U") {
                            this.ufields.splice(a[1], 0, d)
                        }
                    }
                }
            }
            this._generate_trees();
            this._forceRender()
        },
        _clsLeftLine: function (e, b, d) {
            if (typeof d === "undefined") {
                d = 1
            }
            var c = " ";
            var a;
            if (b > 0) {
                a = 0;
                while ((a < this.yfields.length) && (a < e[b].index.length) && (a < e[b - 1].index.length) && (e[b].index[a] === e[b - 1].index[a])) {
                    a++
                }
                a = this.yfields.length - a
            } else {
                a = this.xfields.length > this.yfields.length ? this.xfields.length : this.yfields.length;
                a++
            }
            c += " line_left_" + a;
            return c
        },
        _clsRightLine: function (e, b, d) {
            if (typeof d === "undefined") {
                d = 1
            }
            var c = " ";
            var a;
            if (b + d < e.length) {
                a = 0;
                while ((a < this.yfields.length) && (a < e[b].index.length) && (a < e[b + d].index.length) && (e[b].index[a] === e[b + d].index[a])) {
                    a++
                }
                a = this.yfields.length - a
            } else {
                a = this.xfields.length > this.yfields.length ? this.xfields.length : this.yfields.length;
                a++
            }
            c += " line_right_" + a;
            return c
        },
        _clsTopLine: function (e, d, c) {
            if (typeof c === "undefined") {
                c = 1
            }
            var b = " ";
            var a;
            if (d > 0) {
                a = 0;
                while ((a < this.xfields.length) && (a < e[d].index.length) && (a < e[d - 1].index.length) && (e[d].index[a] === e[d - 1].index[a])) {
                    a++
                }
                a = this.xfields.length - a
            } else {
                a = this.xfields.length > this.yfields.length ? this.xfields.length : this.yfields.length;
                a++
            }
            b += " line_top_" + a;
            return b
        },
        _clsBottomLine: function (e, d, c) {
            if (typeof c === "undefined") {
                c = 1
            }
            var b = " ";
            var a;
            if (d + c < e.length) {
                a = 0;
                while ((a < this.xfields.length) && (a < e[d].index.length) && (a < e[d + c].index.length) && (e[d].index[a] === e[d + c].index[a])) {
                    a++
                }
                a = this.xfields.length - a
            } else {
                a = this.xfields.length > this.yfields.length ? this.xfields.length : this.yfields.length;
                a++
            }
            b += " line_bottom_" + a;
            return b
        },
        _format: function (b, c) {
            var a = null;
            if (this.formatters[c] !== null) {
                a = this.formatters[c].format(b, this.fieldNames[c])
            }
            if ((typeof a === "undefined") || (a === null)) {
                a = this.defaultFormatter.format(b, this.fieldNames[c])
            }
            return a
        }
    })
});

function grp_distinct(b) {
    var a = {};
    a.values = [];
    a.names = {};
    a.fieldtype = "number";
    a.field = b.field;
    if (typeof b.sort === "undefined") {
        a.sort = "ASC"
    } else {
        a.sort = b.sort.toUpperCase()
    } if (typeof b.params !== "undefined") {
        a.params = b.params
    } else {
        a.params = null
    } if (typeof b.showAll !== "undefined") {
        a.showAll = b.showAll
    } else {
        a.showAll = false
    }
    a.CalculateValue = function (e) {
        var c = "";
        var d;
        if (typeof e[this.field] === "function") {
            c = e[this.field](this.params)
        } else {
            if (typeof e[this.field] === "number") {
                c = e[this.field].toString()
            } else {
                if (typeof e[this.field] === "string") {
                    c = e[this.field];
                    this.fieldtype = "string"
                }
            }
        } if (typeof c !== "string") {
            c = ""
        }
        if (typeof this.names[c] !== "undefined") {
            d = this.names[c]
        } else {
            d = this.values.push(c) - 1;
            this.names[c] = d
        }
        return d
    };
    a.getStringValue = function (c) {
        return this.values[c]
    };
    a.DisplayValues = function (f) {
        var e;
        var d;
        if (this.showAll) {
            e = [];
            for (d = 0; d < this.values.length; d++) {
                e.push(d.toString())
            }
        } else {
            e = f.slice(0)
        }
        var c = this;
        if (this.fieldtype === "string") {
            e = e.sort(function (h, g) {
                var i = 0;
                if (c.values[h] < c.values[g]) {
                    i = -1
                }
                if (c.values[h] > c.values[g]) {
                    i = 1
                }
                return i
            })
        } else {
            e = e.sort(function (h, g) {
                var j = parseFloat(c.values[h]);
                var k = parseFloat(c.values[g]);
                var i = 0;
                if (j < k) {
                    i = -1
                }
                if (j > k) {
                    i = 1
                }
                return i
            })
        } if (this.sort === "DESC") {
            e = e.reverse()
        }
        return e
    };
    return a
}
$.unc.plugins.addGrouper("distinct", grp_distinct);

function agregate_average(b) {
    var a = {};
    a.options = $.extend({}, b);
    a.agregate = function (d, c) {
        var e;
        if ((!d) || (d.type !== "agregate_average")) {
            e = {
                type: "agregate_average",
                sum: 0,
                count: 0
            }
        } else {
            e = {
                type: "agregate_average",
                sum: d.sum,
                count: d.count
            }
        } if (c.type === "agregate_average") {
            e.sum += c.sum;
            e.count += c.count
        } else {
            if (typeof c === "object") {
                e.count++;
                if (typeof c[this.options.field] === "number") {
                    e.sum += c[this.options.field]
                } else {
                    if (typeof c[this.options.field] === "string") {
                        try {
                            e.sum += parseInt(c[this.options.field], 10)
                        } catch (f) { }
                    }
                }
            }
        }
        return e
    };
    a.getValue = function (c) {
        var e = null;
        if ((c) && (c.type === "agregate_average") && (c.count > 0)) {
            var d = c.sum / c.count;
            e = d
        }
        return e
    };
    return a
}
$.unc.plugins.addAgregate("average", agregate_average);

function agregate_count() {
    var a = {};
    a.agregate = function (d, c) {
        var e;
        if ((!d) || (d.type !== "agregate_count")) {
            e = {
                type: "agregate_count",
                count: 0
            }
        } else {
            e = {
                type: "agregate_count",
                count: d.count
            }
        } if (c.type === "agregate_count") {
            e.count += c.count
        } else {
            if (typeof c === "object") {
                e.count++
            }
        }
        return e
    };
    a.getValue = function (b) {
        var c = null;
        if ((b) && (b.type === "agregate_count")) {
            c = b.count
        }
        return c
    };
    return a
}
$.unc.plugins.addAgregate("count", agregate_count);

function agregate_distinct(b) {
    var a = {};
    a.field = b.field;
    a.agregate = function (d, c) {
        var e = {
            type: "fa_distinct"
        };
        if ((!d) || (d.type !== "fa_distinct")) {
            e.repeated = false;
            if (c.type === "fa_distinct") {
                e.val = c.val
            } else {
                if (typeof c === "object") {
                    e.repeated = false;
                    if ((typeof c[this.field] === "number") || (typeof c[this.field] === "string")) {
                        e.val = c[this.field]
                    } else {
                        e.val = null
                    }
                } else {
                    e.val = null
                }
            }
        } else {
            if (c.type === "fa_distinct") {
                e.repeated = (d.repeated || c.repeated || (d.val !== c.val));
                e.val = d.val
            } else {
                if (typeof c === "object") {
                    if ((typeof c[this.field] === "number") || (typeof c[this.field] === "string")) {
                        e.repeated = (d.repeated || (d.val !== c[this.field]));
                        e.val = d.val
                    } else {
                        e.repeated = (d.repeated || (d.val !== null));
                        e.val = null
                    }
                } else {
                    e.repeated = (d.repeated || (d.val !== null));
                    e.val = null
                }
            }
        }
        return e
    };
    a.getValue = function (c) {
        var d = null;
        if ((c) && (c.type === "fa_distinct")) {
            if (c.repeated) {
                d = "*"
            } else {
                d = c.val
            }
        }
        return d
    };
    return a
}
$.unc.plugins.addAgregate("distinct", agregate_distinct);

function agregate_sum(b) {
    var a = {};
    a.field = b.field;
    a.agregate = function (d, c) {
        var e;
        if ((!d) || (d.type !== "agregate_sum")) {
            e = {
                type: "agregate_sum",
                sum: 0
            }
        } else {
            e = {
                type: "agregate_sum",
                sum: d.sum
            }
        } if (c.type === "agregate_sum") {
            e.sum += c.sum
        } else {
            if (typeof c === "object") {
                if (typeof c[this.field] === "number") {
                    e.sum += c[this.field]
                } else {
                    if (typeof c[this.field] === "string") {
                        try {
                            e.sum += parseInt(c[this.field], 10)
                        } catch (f) { }
                    }
                }
            }
        }
        return e
    };
    a.getValue = function (c) {
        var d = null;
        if ((c) && (c.type === "agregate_sum")) {
            d = c.sum
        }
        return d
    };
    return a
}
$.unc.plugins.addAgregate("sum", agregate_sum);

function formatter_default() {
    var a = {};
    a.format = function (c) {
        var b = "";
        try {
            b = c.toString()
        } catch (d) { }
        return b
    };
    return a
}
$.unc.plugins.addFormatter("default", formatter_default);