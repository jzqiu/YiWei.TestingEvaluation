!(function (window)
{
    var document = window.document,
        css3 = function (el, key, value)
        {
            var txt = ";";
            ["", "-webkit-", "-moz-", "-ms-"].forEach(function (n) { txt += n + key + ":" + value + ";"; });
            el.style.cssText += txt;
        };

    var hasTouch = 'ontouchstart' in window,
        startEvent = hasTouch ? 'touchstart' : 'mousedown',
        moveEvent = hasTouch ? 'touchmove' : 'mousemove',
        endEvent = hasTouch ? 'touchend' : 'mouseup',
        cancelEvent = hasTouch ? 'touchcancel' : 'mouseup';

    var slide = function (el, options)
    {
        //默认参数
        var option = {
            snapThreshold: 50,//{Number}滑到下一张需要拖动的距离
            time: 300,//{Number}切换时间
            disableScroll: false,//{Bool}是否禁用上下滑动 默认false可以上下滑动
            auto: 0,//{Number}自动滚动时间 默认0不开启自动滚动
            loop: true //{Bool}是否循环滚动 默认开启true
        },
        that = this, undefined,
        wrapper = typeof el === "string" ? document.querySelector(el) : el,//容器
        width = wrapper.clientWidth,//宽
        content = wrapper.querySelector(".slide_content"),//page容器
        pages = wrapper.querySelectorAll(".slide_page"),//页集合
        pageNums = pages.length,//总页数
        multiple = 1,//补齐的倍数
        logIndex = [],//记录滑动的索引顺序
        isDrag = false,//是否在拖动
        startPoint = {},//拖动开始的点
        secondPoint = {},//拖动开始的第二个点
        customEvents = {},//自定义回调函数集
        interval;//定时器

        _init();

        //初始化
        function _init()
        {
            //加载配置参数
            for (var i in options) { option[i] = options[i]; }
            if (pageNums === 0 || pageNums === 1)//不启用图片墙
            {
                wrapper.style.visibility = "visible";
                return false;
            }
            if (!option.loop) { option.auto = 0; }//不开启循环，自动滚动也禁掉
            option.auto = parseInt(option.auto, 10);

            //页面少的时候补齐5个
            if (pageNums === 2)
            {
                content.innerHTML += content.innerHTML + content.innerHTML;
                multiple = 3;
            }
            if (pageNums === 3 || pageNums === 4)
            {
                content.innerHTML += content.innerHTML;
                multiple = 2;
            }
            pages = wrapper.querySelectorAll(".slide_page");
            pageNums = pages.length;

            //初始化page
            content.style.width = width * pageNums + "px";
            [].forEach.call(pages, function (page, i)
            {
                page.style.width = width + "px";
                page.style.left = -width * i + "px";
                move(page, width, 0, 10);
            });
            logIndex = [pageNums - 2, pageNums - 1, 0, 1, 2];//索引记录
            logIndex.forEach(function (n, i)
            {
                move(pages[n], (i - 2) * width, 0, 11);
            });
            window.setTimeout(function ()
            {
                wrapper.style.visibility = "visible";
            }, 100);
            

            //绑定事件
            wrapper.addEventListener(startEvent, _start, false);
            wrapper.addEventListener(moveEvent, _move, false);
            wrapper.addEventListener(endEvent, _end, false);
            window.addEventListener("resize", function ()
            {
                if (width === wrapper.clientWidth) { return; }
                width = wrapper.clientWidth;
                content.style.width = width * pageNums + "px";
                [].forEach.call(pages, function (page, i)
                {
                    page.style.width = width + "px";
                    page.style.left = -width * i + "px";
                    move(page, width, 0, 10);
                });
                logIndex.forEach(function (n, i)
                {
                    move(pages[n], (i - 2) * width, 0, 11);
                });
            }, false);
            scroll();
        };
        //手指按下
        function _start(e)
        {
            isDrag = true; //标示拖动开始
            var point = hasTouch ? e.touches[0] : e;
            startPoint = { x: point.pageX, y: point.pageY };
            secondPoint = undefined;
            stopScroll();
        }
        //手指移动
        function _move(e)
        {
            if (!isDrag) { return false; }
            var point = hasTouch ? e.touches[0] : e;
            if (secondPoint == undefined) { secondPoint = { x: point.pageX, y: point.pageY }; }
            if (!option.disableScroll && (Math.abs(secondPoint.y - startPoint.y) > Math.abs(secondPoint.x - startPoint.x)))//上下滑动趋向
            {
                isDrag = false;
                return false;
            }
            e.preventDefault();

            if (!option.loop && ((that.getIndex() === 0 && (point.pageX - startPoint.x) >= 0) || (that.getIndex() === (that.getNums() - 1) && (point.pageX - startPoint.x) <= 0)))//边界
            {
                move(pages[logIndex[2]], point.pageX - startPoint.x, 0, 11);
            }
            else
            {
                logIndex.forEach(function (n, i)
                {
                    move(pages[n], (point.pageX - startPoint.x) + (i - 2) * width, 0, 11);
                });
            }

        }
        //手指离开
        function _end(e)
        {
            secondPoint = undefined;
            if (!isDrag) { return false; }
            isDrag = false;
            var point = hasTouch ? e.changedTouches[0] : e,
                tempDist = point.pageX - startPoint.x,
                direction = tempDist < 0 ? 1 : -1;//滑动方向 1:left  -1:right 0:回弹
            if ((Math.abs(tempDist) < option.snapThreshold) ||
                (!option.loop && that.getIndex() === 0 && direction === -1) ||
                (!option.loop && that.getIndex() === (that.getNums() - 1) && direction === 1))
            {
                direction = 0;
            }
            animate(direction);
        }
        //滑动
        function animate(direction)
        {
            var k;//需要预加载的索引
            if (direction === 1)//左滑
            {
                logIndex.shift();
                logIndex.push(resetIndex(logIndex[3] + 1));
                k = logIndex[4];
            }
            else if (direction === -1) //右滑
            {
                logIndex.pop();
                logIndex.unshift(resetIndex(logIndex[0] - 1));
                k = logIndex[0];
            }
            logIndex.forEach(function (n, i)
            {
                var b = n === k;
                move(pages[n], (i - 2) * width, b ? 0 : option.time, b ? 10 : 11);
            });
            scroll();
            if (customEvents.flip) { customEvents.flip(that.getIndex()); }
        }

        //page移动
        function move(el, dist, time, zIndex)
        {
            css3(el, "transition", time + "ms");
            css3(el, "transform", "translate3d(" + dist + "px,0,0)");
            if (zIndex) { css3(el, "z-index", zIndex); }
        }

        //重置index
        function resetIndex(index)
        {
            return (index % pageNums + pageNums) % pageNums;
        }

        //自动滚动
        function scroll()
        {
            if (option.auto < 1000) { return; }
            stopScroll();
            interval = window.setTimeout(function ()
            {
                animate(1);
            }, option.auto + option.time);
        }

        //停止滚动
        function stopScroll() { window.clearTimeout(interval); }

        //每次开始切换时执行的回调函数
        this.flip = function (fn) { customEvents.flip = fn; };

        //得到当前页的索引
        this.getIndex = function ()
        {
            var index = logIndex[2], len = that.getNums();
            return (index % len + len) % len;
        };

        //得到总页数
        this.getNums = function ()
        {
            return pageNums / multiple;
        }

        //上一页
        this.prev = function ()
        {
            if (!option.loop && that.getIndex() === 0) { return false; }
            animate(-1);
        };

        //下一页
        this.next = function ()
        {
            if (!option.loop && that.getIndex() === that.getNums() - 1) { return false; }
            animate(1);
        };
    };
    //静态方法初始化图片墙
    slide.init = function (el, option) { return new this(el, option); };

    window.slide = slide;
})(window);
