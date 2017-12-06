//生成页码条方法（方法对象,页码条容器，当前页码，总页数，页码组容量，总行数）
function makePageBar(jsMethodName, pageContainer, pgIndex, pgCount, gpSize, roCount) {
    if (pgCount == 0 || roCount == 0) { pageContainer.innerHTML = ''; return; }
    var groupFirstPageIndex = 0;  //当前页码组的第一个页码
    //var groupCount = 0; //页码组个数
    groupFirstPageIndex = (Math.floor(((pgIndex - 1) / gpSize)) * gpSize) + 1;
    //获得页码组总个数
    //groupCount = Math.ceil(pgCount / gpSize);
    //生成统计数据
    var pagingInfo = " │" + pgIndex + "/" + pgCount + " 共" + roCount + "条";
    var pagingInfo1 = document.createElement("span");
    pagingInfo1.innerHTML = pagingInfo;
    pageContainer.innerHTML = '';
    //生成首页
    //创建li标签
    var homePage = document.createElement("a");
    homePage.href = "javascript:void(0)";
    homePage.onclick = function () {
        if (pgIndex == 1) {
            return false;
        }
        jsMethodName(1, gpSize);
    }
    homePage.innerHTML = "首页";
    pageContainer.appendChild(homePage);
    //生成 上一个页码组 按钮 
    var pagePrevGroup = document.createElement("a");
    if (groupFirstPageIndex > 1) {
        pagePrevGroup.onclick = function () {
            jsMethodName(groupFirstPageIndex - 1, gpSize);
        };
    }
    pagePrevGroup.innerHTML = "上一组";
    var pagePrev = document.createElement("a");
    pagePrev.href = "javascript:void(0)";
    pagePrev.onclick = function () {
        if (pgIndex > 1) {
            jsMethodName(pgIndex - 1, gpSize);
        } else {
            return false;
        }
    };
    pagePrev.innerHTML = "上一页";
    pageContainer.appendChild(pagePrev);

    //按照 页码组容量 和当前页码组 来生成 页码
    var tempI = 0;
    tempI = groupFirstPageIndex;//此时获得的是当前页码组的第一页
    do {
        //页码按钮
        var pageA, pageA_li;
        if (tempI == pgIndex) {//如果 当前生成页码 和 当前访问的页码 相等，则生成 文本，而不是超链接
            pageA = document.createElement("a");
            pageA.href = "javascript:void(0)";
            pageA.innerHTML = tempI;
            pageA.setAttribute("class", "now");
        } else {//否则 生成超链接页码按钮
            pageA = document.createElement("a");
            pageA.href = "javascript:void(0)";
            pageA.setAttribute("pi", tempI);
            pageA.onclick = function () { jsMethodName(this.getAttribute("pi"), gpSize) };
            pageA.innerHTML = tempI;
        }
        pageContainer.appendChild(pageA);
        tempI++;
    } while (tempI < groupFirstPageIndex + gpSize && tempI <= pgCount);//1.不能超过当前页码组最后一个下标 2.不能超过总页数

    //生成下一页
    var pageNext = document.createElement("a");
    pageNext.href = "javascript:void(0)";
    pageNext.onclick = function () {
        //判断 当前页码 是否小于 总页数
        if (pgIndex < pgCount) {
            jsMethodName(pgIndex + 1, gpSize);
        } else {
            return false;
        }
    };
    //pageNext.innerHTML = "Next";
    pageNext.innerHTML = "下一页";
    pageContainer.appendChild(pageNext);

    //生成 NextGroup
    var pageNextGroup = document.createElement("a");
    if (groupFirstPageIndex + gpSize <= pgCount) {
        pageNextGroup.onclick = function () {
            pgIndex = groupFirstPageIndex + gpSize;
            jsMethodName(groupFirstPageIndex + gpSize, gpSize);
        };
    }
    pageNextGroup.innerHTML = "下一组";
    //生成尾页
    var endPage = document.createElement("a");
    endPage.href = "javascript:void(0)";
    endPage.onclick = function () {
        if (pgIndex == pgCount) {
            return false;
        }
        pgIndex = pgCount;
        jsMethodName(pgCount, gpSize);
    };
    endPage.innerHTML = "尾页";
    pageContainer.appendChild(endPage);
    var sel = document.createElement("select");
    sel.id = "selectPageId";
    sel.onchange = function () {
        var pi = this.value;
        jsMethodName(pi, gpSize);
    }
    for (var i = 0; i < pgCount; i++) {
        var opt = new Option("第" + (i + 1) + "页", i + 1);
        if (i == (pgIndex - 1))
            opt.selected = true;
        sel.options.add(opt);
    }
    pageContainer.appendChild(pagingInfo1);
};